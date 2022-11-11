using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace DBHelper
{
    public class ExcelHelper
    {
        /// <summary>
        /// 读取Excel的数据到DataTable。
        /// </summary>
        /// <param name="filePath">Excel 文件路径</param>
        /// <param name="sheetName">要读取的Sheet的名称</param>
        /// <param name="initiDataTable">
        /// 功能说明
        ///     初始化DataTable。
        /// 函数格式：
        ///     static Dictionary<int, string> FunctionName(IRow row, DataTable dt)。
        /// 函数参数说明：
        ///     IRow：是Excel的第一行，也就是索引为0的那一行。
        ///     DataTable：初始化这个Table，这个Table以实例化。只需要在这个DataTable里面加上相对应的列。
        /// 函数返回值的键值对说明：
        ///     Int：是Excel列的索引。
        ///     String：是DataTable的名字。
        /// </param>
        /// <param name="formatDate">
        /// 功能说明：
        ///     格式化数据函数
        /// 函数格式：
        ///     static int FunctionName(DataRow row)
        /// 函数参数说明：
        ///     DataRow：这个参数是当前读取到Excel里面的某一行数据。
        /// 返回值说明：
        ///     int：暂时无用。</param>
        /// <returns></returns>
        public static DataTable ImportExcel(string filePath, string sheetName, Func<IRow, DataTable, Dictionary<int, string>> initiDataTable = null, Func<DataRow, int> formatDate = null)
        {
            DataTable dtExcel = new DataTable();
            try
            {
                IWorkbook hssfworkbook;
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (Path.GetExtension(filePath).Equals(".xls"))
                        hssfworkbook = new HSSFWorkbook(file);
                    else if (Path.GetExtension(filePath).Equals(".xlsx"))
                        hssfworkbook = new XSSFWorkbook(file);
                    else
                        throw new Exception(string.Format("文件【{0}】不是一个Excel文件", filePath));
                    file.Close();
                    GC.Collect();
                }
                ISheet sheet = hssfworkbook.GetSheet(sheetName);
                IRow row = sheet.GetRow(0);
                if (initiDataTable == null)
                {
                    initiDataTable = InitializeTable;
                }
                Dictionary<int, string> colList = initiDataTable(row, dtExcel);

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);
                    DataRow dtRow = dtExcel.NewRow();
                    if (row == null)
                        continue;

                    foreach (KeyValuePair<int, string> keyValue in colList)
                    {
                        try
                        {
                            ICell cell = row.GetCell(keyValue.Key);
                            SetDataTableCellValue(cell, dtRow, keyValue.Value);
                        }
                        catch
                        {
                        }
                    }
                    if (formatDate != null)
                        formatDate(dtRow);
                    dtExcel.Rows.Add(dtRow);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtExcel;
        }

        private static void SetDataTableCellValue(ICell cell, DataRow row, string colName)
        {
            switch (cell.CellType)
            {
                case CellType.String:
                    string str = cell.StringCellValue;
                    if (str != null && str.Length > 0)
                    {
                        row[colName] = str.ToString();
                    }
                    else
                    {
                        row[colName] = DBNull.Value;
                    }
                    break;
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        row[colName] = DateTime.FromOADate(cell.NumericCellValue);
                        //row[colName] = cell.DateCellValue;
                    }
                    //自定义时间格式处理 58 m月d日，57 yyyy年m月，31 yyyy年m月d日 , 32 h时m分
                    else if (cell.CellStyle.DataFormat == 58 ||
                        cell.CellStyle.DataFormat == 57 ||
                        cell.CellStyle.DataFormat == 31 ||
                        cell.CellStyle.DataFormat == 32)
                    {
                        row[colName] = DateTime.FromOADate(cell.NumericCellValue);
                    }
                    else
                    {
                        row[colName] = cell.NumericCellValue;
                    }
                    break;
                case CellType.Boolean:
                    row[colName] = cell.BooleanCellValue;
                    break;
                case CellType.Error:
                    break;
                case CellType.Formula:
                    switch (cell.CachedFormulaResultType)
                    {
                        case CellType.String:
                            string strFORMULA = cell.StringCellValue;
                            if (strFORMULA != null && strFORMULA.Length > 0)
                            {
                                row[colName] = strFORMULA.ToString();
                            }
                            else
                            {
                                row[colName] = DBNull.Value;
                            }
                            break;
                        case CellType.Numeric:
                            row[colName] = cell.NumericCellValue;
                            break;
                        case CellType.Boolean:
                            row[colName] = cell.BooleanCellValue;
                            break;
                        case CellType.Error:
                            break;
                        default:
                            row[colName] = DBNull.Value;
                            break;
                    }
                    break;
                default:
                    row[colName] = DBNull.Value;
                    break;
            }
        }

        private static Dictionary<int, string> InitializeTable(IRow row, DataTable dt)
        {
            Dictionary<int, string> colList = new Dictionary<int, string>();
            int cellCount = row.LastCellNum;

            for (int i = row.FirstCellNum; i < cellCount; i++)
            {
                string colName = string.Format("Columns{0}", i);
                dt.Columns.Add(new DataColumn(colName));
                colList.Add(i, colName);
            }
            return colList;
        }
    }
}

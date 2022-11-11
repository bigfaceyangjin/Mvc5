using MySql.Data.MySqlClient;
using NHibernate;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DBHelper.Entitys
{
    /// <summary>
    /// 运单成本
    /// </summary>
    public class WayBillCost : BaseEntity<WayBillCost, int>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <returns></returns>
        public static int Synchro()
        {
            int sourceCount = 0;
            try
            {
                string orderCostPath = System.Configuration.ConfigurationManager.AppSettings["OrderCostPath"].ToString();//源文件路径
                string filePathBak = System.Configuration.ConfigurationManager.AppSettings["FileBakPath"].ToString();//备份文件路径目标
                if (!Directory.Exists(orderCostPath))
                    Directory.CreateDirectory(orderCostPath);
                if (!Directory.Exists(filePathBak))
                    Directory.CreateDirectory(filePathBak);

                StringBuilder errorMsg = new StringBuilder();
                string[] fileList = Directory.GetFiles(orderCostPath, "*.xls");
                if (fileList.Length > 0)
                {
                    //InitializeTable();
                    string conn = string.Empty;

                    string currFileDate = string.Empty;
                    int index = 0;
                    List<string> backupFile = new List<string>();
                    foreach (string filePath in fileList)
                    {
                        try
                        {
                            string fileName = Path.GetFileNameWithoutExtension(filePath);
                            string fileDate = string.Empty;
                            if (fileName.Length >= 8)
                                fileDate = fileName.Substring(0, 8);
                            if (index == 0)
                            {
                                currFileDate = fileDate;
                                // 导入Excel的数据到数据库
                                ImportExcelToDB(filePath, fileDate);
                                backupFile.Add(filePath);
                            }
                            else
                            {
                                if (currFileDate.Equals(fileDate))
                                {
                                    // 导入Excel的数据到数据库
                                    ImportExcelToDB(filePath, fileDate);
                                    backupFile.Add(filePath);
                                }
                                else
                                {
                                    // 验证并处理数据
                                    VerificationData();
                                    // 备份文件
                                    BackupExcelFile(backupFile, filePathBak);
                                    backupFile.Clear();
                                    // 导入Excel的数据到数据库
                                    ImportExcelToDB(fileDate, fileDate);
                                    backupFile.Add(filePath);
                                    currFileDate = fileDate;
                                }
                            }
                            if (index == fileList.Length - 1)
                            {
                                // 验证并处理数据
                                VerificationData();
                                // 备份文件
                                BackupExcelFile(backupFile, filePathBak);
                                backupFile.Clear();
                            }
                            index++;
                        }
                        catch (Exception ex)
                        {
                            errorMsg.Append(ex.Message);
                        }
                    }
                }

                if (errorMsg.Length > 0)
                {
                    throw new Exception(errorMsg.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sourceCount;
        }

        /// <summary>
        /// 把Excel导入到数据库
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileDate"></param>
        private static void ImportExcelToDB(string filePath, string fileDate)
        {
            using (ISession session = NHibernateSession.SessionFactory.OpenSession())
            {
                using (var trans = (session.Connection.BeginTransaction()))
                {
                    try
                    {
                        // 把Excel的数据导入到DataTable
                        DataTable dtExcel = ExcelHelper.ImportExcel(filePath, "运单成本", new Func<IRow, DataTable, Dictionary<int, string>>(InitializeTable), new Func<DataRow, int>(FormatDate));
                        string sql = "SELECT * FROM Temp_WaybillExcel";
                        // 把DataTable的数据保存到数据库
                        AdapterUpdate(sql, dtExcel, session.Connection, trans);

                        if (IsNumeric(fileDate))
                        {
                            DateTime reconcileDate;
                            if (DateTime.TryParseExact(fileDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out reconcileDate))
                            {
                                string strSql = "UPDATE Temp_WaybillExcel SET ReconcileDate = @ReconcileDate";
                                List<DbParameter> paraList = new List<DbParameter>(){
                                new MySqlParameter("@ReconcileDate", reconcileDate)
                            };
                                ExecuteNonQuery(strSql, session.Connection, trans, paraList);
                            }
                        }

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (trans != null)
                            trans.Rollback();
                        //throw ex;
                        session.CreateSQLQuery("TRUNCATE TABLE Temp_WaybillExcel").ExecuteUpdate();
                        throw new Exception(string.Format("导入文件【{0}】出错。\r\n错误：{1}", filePath, ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        private static void VerificationData()
        {
            using (ISession session = NHibernateSession.SessionFactory.OpenSession())
            {
                try
                {
                    // 执存储过程处理数据
                    var query = session.CreateSQLQuery("CALL p_ImportWayBillCost()");
                    query.SetTimeout(600);
                    query.ExecuteUpdate();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("执行存储过程时出错。\r\n错误：{0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// 备份Excel文件
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="filePathBak"></param> 
        private static void BackupExcelFile(List<string> fileList, string filePathBak)
        {
            foreach (string filePath in fileList)
                File.Move(filePath, Path.Combine(filePathBak, DateTime.Now.ToString("yyyyMMddHHmm") + "_" + Path.GetFileName(filePath)));
        }

        /// <summary>
        /// 判断是否为一个数字并反回值
        /// </summary>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static bool IsNumeric(String message)
        {
            Regex rex = new Regex(@"^[-]?\d+[.]?\d*$");
            if (rex.IsMatch(message))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 格式化数据，处理批次列包含“包”的数据。
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static int FormatDate(DataRow row)
        {
            if (row != null)
            {
                if (row.Table.Columns.Contains("BatchNO"))
                {
                    object obj = row["BatchNO"];
                    string value = obj != DBNull.Value && obj != null ? obj.ToString() : "";
                    row["BatchNO"] = value.Replace("包", "");
                }
            }
            return 1;
        }
        /// <summary>
        /// 初始化DataTable和配置Excel列
        /// </summary>
        private static Dictionary<int, string> InitializeTable(IRow row, DataTable dt)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            try
            {
                string newName = string.Empty;
                for (int i = 0; i <= 7; i++)
                {
                    ICell cell = row.GetCell(i);
                    if (cell == null)
                        throw new Exception("Excel的格式不正确！");
                    string colName = cell.StringCellValue.Trim();
                    switch (colName)
                    {
                        case "收寄日期":
                            newName = "PostingTime";
                            dt.Columns.Add(new DataColumn(newName));
                            result.Add(0, newName);
                            break;
                        case "邮件号":
                            newName = "ExpressNo";
                            dt.Columns.Add(new DataColumn(newName));
                            result.Add(1, newName);
                            break;
                        case "寄达地":
                            newName = "SendAddress";
                            dt.Columns.Add(new DataColumn(newName));
                            result.Add(2, newName);
                            break;
                        case "重量":
                            newName = "Weight";
                            dt.Columns.Add(new DataColumn(newName));
                            result.Add(3, newName);
                            break;
                        case "邮资":
                            newName = "WayBillFee";
                            dt.Columns.Add(new DataColumn(newName));
                            result.Add(4, newName);
                            break;
                        case "邮件处理费":
                            newName = "ProcessingFee";
                            dt.Columns.Add(new DataColumn(newName));
                            result.Add(5, newName);
                            break;
                        case "产品":
                            newName = "Product";
                            dt.Columns.Add(new DataColumn(newName));
                            result.Add(6, newName);
                            break;
                        case "批次":
                            newName = "BatchNO";
                            dt.Columns.Add(new DataColumn(newName));
                            result.Add(7, newName);
                            break;
                        default:
                            throw new Exception("Excel的格式不正确！");
                    }
                }
                newName = "ReconcileDate";
                DataColumn col = new DataColumn(newName, typeof(System.DateTime));
                col.DefaultValue = DBNull.Value;
                dt.Columns.Add(col);

                newName = "IsImport";
                col = new DataColumn(newName, typeof(System.Boolean));
                col.DefaultValue = false;
                dt.Columns.Add(col);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// 用适配器保存DataTable的数据
        /// </summary>
        /// <param name="selectString"></param>
        /// <param name="table"></param>
        /// <param name="connection"></param>
        /// <param name="trans"></param>
        public static void AdapterUpdate(string selectString, DataTable table, IDbConnection connection, IDbTransaction trans)
        {
            try
            {
                MySqlDataAdapter Adapter = new MySqlDataAdapter();
                Adapter.SelectCommand = new MySqlCommand(selectString, connection as MySqlConnection);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(Adapter);
                Adapter.InsertCommand = builder.GetInsertCommand();
                if (trans != null)
                {
                    Adapter.InsertCommand.Transaction = trans as MySqlTransaction;
                }
                Adapter.Update(table);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connection"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, IDbConnection connection, IDbTransaction trans)
        {
            int result = ExecuteNonQuery(sql, connection, trans, null);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connection"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, IDbConnection connection, IDbTransaction trans, List<DbParameter> parameterList)
        {
            int result = 0;
            MySqlCommand cmd = null;
            try
            {
                cmd = new MySqlCommand(sql);
                cmd.Connection = connection as MySqlConnection;
                cmd.Transaction = trans as MySqlTransaction;
                cmd.CommandType = CommandType.Text;
                if (parameterList != null && parameterList.Count > 0)
                {
                    foreach (DbParameter p in parameterList)
                        cmd.Parameters.Add(p as MySqlParameter);
                }
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Clone();
            }
            return result;
        }
    }
}

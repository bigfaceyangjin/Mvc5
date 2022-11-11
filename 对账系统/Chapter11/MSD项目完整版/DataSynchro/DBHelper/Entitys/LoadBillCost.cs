using NHibernate;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace DBHelper.Entitys
{
    /// <summary>
    /// 提单成本
    /// </summary>
    public class LoadBillCost : BaseEntity<LoadBillCost, int>
    {
        //static Dictionary<int, string> colList = null;
        //static DataTable dtExcel = null;
        /// <summary>
        /// 根据提单号查询提单成本是否有重复记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static DataTable GetByLoadBillNum(string loadBillNum)
        {
            //var query = NHibernateSession.CreateQuery("from LoadBillCost where LoadBillNum =:LoadBillNum");
            //ISession NHibernateSession _session=new 
            DataTable dt = new DataTable();
            using (ISession i = NHibernateSession.GetSession(EntityMode.Map))
            {
                IDbCommand cmd = i.Connection.CreateCommand();
                cmd.CommandText = @"select * from LoadBillCost where LoadBillNum ='" + loadBillNum + "'";
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }


        /// <summary>
        /// 根据提单号查询提单成本是否有重复记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static DataTable GetLoadBillInComeByLoadBillNum(string loadBillNum)
        {
            //var query = NHibernateSession.CreateQuery("from LoadBillCost where LoadBillNum =:LoadBillNum");
            //ISession NHibernateSession _session=new 
            DataTable dt = new DataTable();
            using (ISession i = NHibernateSession.GetSession(EntityMode.Map))
            {
                IDbCommand cmd = i.Connection.CreateCommand();
                cmd.CommandText = @"select * from LoadBillInCome where LoadBillNum ='" + loadBillNum + "'";
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <returns></returns>
        public static int Synchro()
        {
            int sourceCount = 0;
            try
            {
                string billCostPath = System.Configuration.ConfigurationManager.AppSettings["BillCostPath"].ToString();//源文件路径

                string filePathBak = System.Configuration.ConfigurationManager.AppSettings["FileBakPath"].ToString();//备份文件路径目标
                string ConfigPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NHibernate.config");
                IStatelessSession session = NHibernateSessionManager.Instance.GetNewSessionFrom(ConfigPath);


                string filePathOrder = System.Configuration.ConfigurationManager.AppSettings["BillCostPath"].ToString();// @"\Uploads\OrderCost";    //取到放的路径配置文件里面取

                //判断该文件夹里面是否有文件
                if (Directory.GetFiles(filePathOrder).Length > 0)
                {
                    //获取所有文件
                    string[] files = Directory.GetFiles(filePathOrder);
                    //循环读取文件
                    foreach (string file in files)
                    {
                        //读取execl文件
                        //IWorkbook workbook = null;
                        //var fileExten = Path.GetExtension(file);
                        //var fsRead = File.OpenRead(file);      //读取保存路径文件 
                        //if (fileExten == ".xls")
                        //{
                        //    workbook = new HSSFWorkbook(fsRead);
                        //}
                        //else if (fileExten == ".xlsx")
                        //{
                        //    workbook = new XSSFWorkbook(fsRead);
                        //}
                        //else
                        //{
                        //    return sourceCount;
                        //}
                        string strsql = pingSqlNew(System.IO.Path.GetFileName(file));
                        if (strsql != null && strsql != "" && System.IO.Path.GetFileName(file) != null && System.IO.Path.GetFileName(file) != "")
                        {
                            int ExecuCount = session.CreateSQLQuery(strsql).ExecuteUpdate();
                            //判断执行是否成功
                            if (ExecuCount > 0)
                            {
                                //string tt = orderCostPath + @"\" + filepath;
                                //string tot = filePathBak + @"\" + filepath;
                                //移动文件                
                                File.Move(billCostPath + @"\" + System.IO.Path.GetFileName(file), filePathBak + @"\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + System.IO.Path.GetFileName(file));
                            }
                            sourceCount += ExecuCount;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return sourceCount;
        }

        /// <summary>
        /// 添加数据的sql 语句
        /// </summary>
        /// <returns></returns>
        public static string pingSql(string fileName)
        {
            // 文件保存路径
            string strsql = "";
            string strpath = "";

            string filePathOrder = System.Configuration.ConfigurationManager.AppSettings["BillCostPath"].ToString() + @"\" + fileName; ;// @"\Uploads\OrderCost";    //取到放的路径配置文件里面取

            //读取execl文件
            IWorkbook workbook = null;
            var fileExten = Path.GetExtension(filePathOrder);
            var fsRead = File.OpenRead(filePathOrder);      //读取保存路径文件 
            if (fileExten == ".xls")
            {
                workbook = new HSSFWorkbook(fsRead);
            }
            else if (fileExten == ".xlsx")
            {
                workbook = new XSSFWorkbook(fsRead);
            }
            else
            {
                return null;
            }
            StringBuilder sqlstr = new StringBuilder();
            string sqlHead = "insert into LoadBillCost (LoadBillNum,LoadTime,GroundHandlingFee,StoreFee,CreateTime,DeliveryTime,FactWeight,FeeWeight,TotalFee,StorageFeeReason,ReconcileDate,`Status`) values  ";
            //读取到execl数据
            fsRead.Close();
            var lackProvinceCount = 1;
            var sheetCount = workbook.NumberOfSheets;
            for (int sheetIndex = 0; sheetIndex < sheetCount; sheetIndex++)
            {
                var sheetIndexShow = sheetIndex + 1;
                var sheet = workbook.GetSheetAt(sheetIndex);
                //IsCalculating = true;
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    var strdate = row.GetCell(0).ToString(); //最后一行是合计不用加到数据库
                    if (strdate != "合计")
                    {

                        //地勤费 --有公式
                        string dqf;
                        //判断类型
                        switch (row.GetCell(6).CachedFormulaResultType)
                        {
                            case CellType.String:
                                dqf = row.GetCell(6).StringCellValue;
                                break;
                            case CellType.Numeric:
                                dqf = Convert.ToString(row.GetCell(6).NumericCellValue);
                                break;
                            case CellType.Boolean:
                                dqf = Convert.ToString(row.GetCell(6).BooleanCellValue);
                                break;
                            default:
                                dqf = "";
                                break;
                        }

                        //总合计 --有公式
                        string zhj;
                        //判断类型
                        switch (row.GetCell(8).CachedFormulaResultType)
                        {
                            case CellType.String:
                                zhj = row.GetCell(8).StringCellValue;
                                break;
                            case CellType.Numeric:
                                zhj = Convert.ToString(row.GetCell(8).NumericCellValue);
                                break;

                            case CellType.Boolean:
                                zhj = Convert.ToString(row.GetCell(8).BooleanCellValue);
                                break;
                            default:
                                zhj = "";
                                break;
                        }

                        //到货时间 --有公式
                        DateTime dhtime = new DateTime();
                        if (row.Cells[1].DateCellValue != null)
                        {
                            dhtime = row.Cells[1].DateCellValue;
                        }
                        //提货时间 --有公式
                        DateTime thtime = new DateTime();
                        if (row.Cells[2].DateCellValue != null)
                        {
                            thtime = row.Cells[2].DateCellValue;
                        }
                        string loadBillNum = row.GetCell(3).ToString();
                        //判断类型          
                        int statuc;
                        //1判断提单收入表是否存在该提单 未匹配为2
                        DataTable dt2 = LoadBillCost.GetLoadBillInComeByLoadBillNum(loadBillNum);
                        if (dt2.Rows.Count > 0)
                        {
                            //2判断提单草本表是否重复提单号 重复状态为1                
                            DataTable dt = LoadBillCost.GetByLoadBillNum(loadBillNum);
                            if (dt.Rows.Count > 0)
                            {
                                statuc = 1;
                                sqlstr.AppendLine(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'),", row.GetCell(3), thtime, dqf, row.GetCell(7), DateTime.Now, dhtime, row.GetCell(4), row.GetCell(5), zhj, row.GetCell(9), System.IO.Path.GetFileName(fileName).Substring(0, 8), statuc));
                            }
                            else
                            {
                                statuc = 0;
                                sqlstr.AppendLine(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'),", row.GetCell(3), thtime, dqf, row.GetCell(7), DateTime.Now, dhtime, row.GetCell(4), row.GetCell(5), zhj, row.GetCell(9), System.IO.Path.GetFileName(fileName).Substring(0, 8), statuc));
                            }
                        }
                        else
                        {
                            //成本数据没有
                            statuc = 2;
                            sqlstr.AppendLine(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'),", row.GetCell(3), thtime, dqf, row.GetCell(7), DateTime.Now, dhtime, row.GetCell(4), row.GetCell(5), zhj, row.GetCell(9), System.IO.Path.GetFileName(fileName).Substring(0, 8), statuc));
                        }
                        //3判断Execl是否存在提单号重复的 值为 3
                    }
                }
            }
            //strpath = System.IO.Path.GetFileName(file);
            strsql = string.Format("{0} {1}", sqlHead, sqlstr.ToString().Substring(0, sqlstr.Length - 3));
            return strsql;
        }


        /// <summary>
        /// 添加数据的sql 语句
        /// </summary>
        /// <returns></returns>
        public static string pingSqlNew(string fileName)
        {
            // 文件保存路径
            string strsql = "";
            StringBuilder sqlstr = new StringBuilder();
            string strpath = "";

            string filePathOrder = System.Configuration.ConfigurationManager.AppSettings["BillCostPath"].ToString() + @"\" + fileName; ;// @"\Uploads\OrderCost";    //取到放的路径配置文件里面取
            //读取数据到表中
            DataTable dtExcel = ExcelHelper.ImportExcel(filePathOrder, "提单成本", new Func<IRow, DataTable, Dictionary<int, string>>(InitializeTable), null);
            //dtExcel = null;
            string sqlHead = "insert into LoadBillCost (LoadBillNum,LoadTime,GroundHandlingFee,StoreFee,CreateTime,DeliveryTime,FactWeight,FeeWeight,TotalFee,StorageFeeReason,ReconcileDate,`Status`) values  ";
            //方法2
            //DataView dv = dtExcel.DefaultView;
            //DataTable dt = dv.ToTable("NewTable", false, "LoadBillNum");
            int statuc = 0;
            foreach (DataRow dr in dtExcel.Rows)
            {
                //判断td是否有重复记录
                DataRow[] drws = dtExcel.Select(string.Format("LoadBillNum = '{0}'", dr["LoadBillNum"].ToString()));// dtExcel.Select(dr["LoadBillNum"].ToString());
                if (drws.Length > 1)
                {
                    //excel有重复
                    //dr["Status"] = 4;               
                    //1判断提单收入表是否存在该提单 未匹配为2
                    DataTable dt2 = LoadBillCost.GetLoadBillInComeByLoadBillNum(dr["LoadBillNum"].ToString());
                    if (dt2.Rows.Count > 0)
                    {
                        //2判断提单草本表是否重复提单号 重复状态为1                
                        DataTable dt = LoadBillCost.GetByLoadBillNum(dr["LoadBillNum"].ToString());
                        if (dt.Rows.Count > 0)
                        {
                            statuc = 4;//1;                                                                                                                                                                                                                                                                                                ,,                                              
                            sqlstr.AppendLine(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'),", dr["LoadBillNum"].ToString(), dr["LoadTime"].ToString(), dr["GroundHandlingFee"].ToString(), dr["StoreFee"].ToString(), DateTime.Now, dr["DeliveryTime"].ToString(), dr["FactWeight"].ToString(), dr["FeeWeight"].ToString(), dr["TotalFee"].ToString(), dr["StorageFeeReason"].ToString(), System.IO.Path.GetFileName(fileName).Substring(0, 8), statuc));
                        }
                        else
                        {
                            statuc = 3;
                            sqlstr.AppendLine(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'),", dr["LoadBillNum"].ToString(), dr["LoadTime"].ToString(), dr["GroundHandlingFee"].ToString(), dr["StoreFee"].ToString(), DateTime.Now, dr["DeliveryTime"].ToString(), dr["FactWeight"].ToString(), dr["FeeWeight"].ToString(), dr["TotalFee"].ToString(), dr["StorageFeeReason"].ToString(), System.IO.Path.GetFileName(fileName).Substring(0, 8), statuc));
                        }
                    }
                    else
                    {
                        //成本数据没有
                        statuc = 5;//2;
                        sqlstr.AppendLine(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'),", dr["LoadBillNum"].ToString(), dr["LoadTime"].ToString(), dr["GroundHandlingFee"].ToString(), dr["StoreFee"].ToString(), DateTime.Now, dr["DeliveryTime"].ToString(), dr["FactWeight"].ToString(), dr["FeeWeight"].ToString(), dr["TotalFee"].ToString(), dr["StorageFeeReason"].ToString(), System.IO.Path.GetFileName(fileName).Substring(0, 8), statuc));
                    }
                }
                //excel没有重复
                else
                {
                    //1判断提单收入表是否存在该提单 未匹配为2
                    DataTable dt2 = LoadBillCost.GetLoadBillInComeByLoadBillNum(dr["LoadBillNum"].ToString());
                    if (dt2.Rows.Count > 0)
                    {
                        //2判断提单草本表是否重复提单号 重复状态为1                
                        DataTable dt = LoadBillCost.GetByLoadBillNum(dr["LoadBillNum"].ToString());
                        if (dt.Rows.Count > 0)
                        {
                            statuc = 1;
                            sqlstr.AppendLine(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'),", dr["LoadBillNum"].ToString(), dr["LoadTime"].ToString(), dr["GroundHandlingFee"].ToString(), dr["StoreFee"].ToString(), DateTime.Now, dr["DeliveryTime"].ToString(), dr["FactWeight"].ToString(), dr["FeeWeight"].ToString(), dr["TotalFee"].ToString(), dr["StorageFeeReason"].ToString(), System.IO.Path.GetFileName(fileName).Substring(0, 8), statuc));
                        }
                        else
                        {
                            statuc = 0;
                            sqlstr.AppendLine(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'),", dr["LoadBillNum"].ToString(), dr["LoadTime"].ToString(), dr["GroundHandlingFee"].ToString(), dr["StoreFee"].ToString(), DateTime.Now, dr["DeliveryTime"].ToString(), dr["FactWeight"].ToString(), dr["FeeWeight"].ToString(), dr["TotalFee"].ToString(), dr["StorageFeeReason"].ToString(), System.IO.Path.GetFileName(fileName).Substring(0, 8), statuc));
                        }
                    }
                    else
                    {
                        //成本数据没有
                        statuc = 2;
                        sqlstr.AppendLine(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'),", dr["LoadBillNum"].ToString(), dr["LoadTime"].ToString(), dr["GroundHandlingFee"].ToString(), dr["StoreFee"].ToString(), DateTime.Now, dr["DeliveryTime"].ToString(), dr["FactWeight"].ToString(), dr["FeeWeight"].ToString(), dr["TotalFee"].ToString(), dr["StorageFeeReason"].ToString(), System.IO.Path.GetFileName(fileName).Substring(0, 8), statuc));
                    }
                }
            }
            //
            strsql = string.Format("{0} {1}", sqlHead, sqlstr.ToString().Substring(0, sqlstr.Length - 3));
            return strsql;
        }
        /// <summary>
        /// 初始化DataTable和配置Excel列
        /// </summary>
        private static Dictionary<int, string> InitializeTable(IRow row, DataTable dt)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            result = new Dictionary<int, string>();
            // 到货日期, Excel B列
            string colName = "DeliveryTime";
            dt.Columns.Add(new DataColumn(colName, typeof(System.DateTime)));
            result.Add(1, colName);

            // 提货日期, Excel C列
            colName = "LoadTime";
            dt.Columns.Add(new DataColumn(colName, typeof(System.DateTime)));
            result.Add(2, colName);

            // 航空提单号, Excel D列
            colName = "LoadBillNum";
            dt.Columns.Add(new DataColumn(colName));
            result.Add(3, colName);

            // 实重, Excel E列
            colName = "FactWeight";
            dt.Columns.Add(new DataColumn(colName));
            result.Add(4, colName);

            // 计费重量(kg) , ExcelF列
            colName = "FeeWeight";
            dt.Columns.Add(new DataColumn(colName));
            result.Add(5, colName);

            // 地勤费(0.9元/KG）, Excel G列
            colName = "GroundHandlingFee";
            dt.Columns.Add(new DataColumn(colName));
            result.Add(6, colName);

            // 仓租, Excel H列
            colName = "StoreFee";
            dt.Columns.Add(new DataColumn(colName));
            result.Add(7, colName);

            // 总成本, Excel I列
            colName = "TotalFee";
            dt.Columns.Add(new DataColumn(colName));
            result.Add(8, colName);

            // 产生仓租原因, Excel J列
            colName = "StorageFeeReason";
            dt.Columns.Add(new DataColumn(colName));
            result.Add(9, colName);

            // 状态, Excel K列
            colName = "Status";
            dt.Columns.Add(new DataColumn(colName));
            result.Add(10, colName);

            return result;
        }
    }
}

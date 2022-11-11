using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBHelper.Extend;
using System.Data;

namespace DBHelper.Entitys
{
    public class ExpressCurInfo : BaseEntity<ExpressCurInfo, int>
    {
        #region property
        /// <summary>
        /// 收货商ID
        /// </summary>
        public virtual int BusinessID{get;set;}
        /// <summary>
        /// 收货商名
        /// </summary>
        public virtual string BusinessName{get;set;}
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName{get;set;}
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        #endregion

        #region common method
        /// <summary>
        /// 根据收货商ID获取收货商数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static IList<ExpressCurInfo> GetByBusinessID(List<int> ids)
        {
            var query = NHibernateSession.CreateQuery("from ExpressCurInfo where BusinessID in (:ID)");
            query.SetParameterList("ID", ids);
            return query.List<ExpressCurInfo>();
        }

        /// <summary>
        /// 根据收货商ID获取客户ID
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetCustomers(List<int> ids)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            var query = NHibernateSession.CreateSQLQuery("SELECT DISTINCT DeliveryID,CusID from ExpressCurInfo where DeliveryID IN (:ID) and CusID is not null;");
            query.SetParameterList("ID", ids);
            IList<object[]> li=query.List<object[]>();
            foreach (object[] item in li)
            {
                result.Add(int.Parse(item[0].ToString()),item[1].ToString());
            }
            return result;
        }

        public static int Synchro()
        {
            int sourceCount = 0;
            string ConfigPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NHibernate.config");
            List<DBHelper.Entitys.ExpressCurInfo> sqlbatchparameter = new List<DBHelper.Entitys.ExpressCurInfo>();
            List<DBHelper.Entitys.ExpressCurInfo> li = DBHelper.Entitys.ExpressCurInfo.GetAllBySource();
            if (li.Count > 0)
            {
                using (IStatelessSession session = NHibernateSessionManager.Instance.GetNewSessionFrom(ConfigPath))
                {
                    IList<DBHelper.Entitys.ExpressCurInfo> lisource = DBHelper.Entitys.ExpressCurInfo.GetByBusinessID(li.Select(a => a.BusinessID).ToList<int>());
                    IEnumerable<DBHelper.Entitys.ExpressCurInfo> detailli = from cc in li where lisource.Where(a => a.BusinessID == cc.BusinessID).Count() == 0 select cc;
                    DateTime dt = DateTime.Now;
                    foreach (var item in detailli)
                    {
                        item.CreateTime = dt;
                        sqlbatchparameter.Add(item);
                    }
                    using (IDbTransaction tran = session.Connection.BeginTransaction())
                    {
                        IDbCommand cmd = session.Connection.CreateCommand(); ;
                        try
                        {
                            if (sqlbatchparameter.Count() > 0)
                            {
                                cmd.Transaction = tran;
                                cmd.CommandText = GetbathInsertSql(sqlbatchparameter);
                                cmd.ExecuteNonQuery();
                                DBHelper.Entitys.ExpressCurInfo.EditBySource(li.Select(o => o.BusinessID).ToList());
                                tran.Commit();
                            }
                            sourceCount = li.Count;
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            return sourceCount;
        }

        public static string GetbathInsertSql(IEnumerable<ExpressCurInfo> li)
        {
            StringBuilder sqlValue = new StringBuilder();
            string sqlHead = @"INSERT INTO ExpressCurInfo(DeliveryID,DeliveryName,AccountName,CreateTime) VALUES";
            foreach (var item in li)
            {
                sqlValue.AppendLine(string.Format("({0},{1},{2},{3}),",item.BusinessID, item.BusinessName.ToSqlStr(), item.UserName.ToSqlStr(), item.CreateTime.ToDateStr()));
            }
            return string.Format("{0} {1}", sqlHead, sqlValue.ToString().Substring(0, sqlValue.Length - 3));
        }
        #endregion

        #region entity method
        #endregion

        public static void Insert()
        {
            StringBuilder sqlbatchparameter = new StringBuilder();
            string ConfigPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NHibernate.config");
            using (IStatelessSession session = NHibernateSessionManager.Instance.GetNewSessionFrom(ConfigPath))
            {
                try
                {
                    IDbTransaction tran = session.Connection.BeginTransaction();
                    try
                    {
                        for (int i = 0; i < 10000; i++)
                        {
                            IDbCommand cmd = session.Connection.CreateCommand();
                            cmd.Transaction = tran;
                            cmd.CommandText = string.Format("INSERT INTO RTest(GuID) values ('{0}');", Guid.NewGuid());
                            cmd.ExecuteNonQuery();
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        session.Connection.Close();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}

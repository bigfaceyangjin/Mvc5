using Iesi.Collections.Generic;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBHelper.Extend;
using System.Data;

namespace DBHelper.Entitys
{
    public class WayBillInfo : BaseEntity<WayBillInfo, int>
    {
        #region proprety
        /// <summary>
        /// 详细地址
        /// </summary>
        public virtual string Address{get;set;}
        /// <summary>
        /// 收货商ID
        /// </summary>
        public virtual int BusinessId{get;set;}
        /// <summary>
        /// 公司ID
        /// </summary>
        public virtual int? CompanyId{get;set;}
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime? CreateTime{get;set;}
        /// <summary>
        /// 运单号
        /// </summary>
        public virtual string ExpressNo{get;set;}
        /// <summary>
        /// 业务系统订单号ID
        /// </summary>
        public virtual int OrderId{get;set;}
        /// <summary>
        /// 收件人省份
        /// </summary>
        public virtual string Province{get;set;}
        /// <summary>
        /// 收件人
        /// </summary>
        public virtual string Receiver{get;set;}
        /// <summary>
        /// 包裹重量
        /// </summary>
        public virtual decimal Weight { get; set; }
        /// <summary>
        /// 提货单ID
        /// </summary>
        public virtual int LoadBillID { get; set; }
        /// <summary>
        /// 提单号
        /// </summary>
        public virtual string LoadBillNO { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderCode { get; set; }
        /// <summary>
        /// 发件人省份(默认广州省)
        /// </summary>
        public virtual string SenderProvince { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public virtual int? CustomerID { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public virtual decimal ExpressFee { get; set; }
        #endregion

        #region common method
        /// <summary>
        /// 根据业务系统的ID获取运单数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static IList<WayBillInfo> GetByBussinessID(IEnumerable<int> ids)
        {
            var query = NHibernateSession.CreateQuery("from WayBillInfo where OrderId in (:ID)");
            query.SetParameterList("ID", ids);
            return query.List<WayBillInfo>();
        }

        public static PageOfOrder GetFilterBySource(Orderfilter filter)
        {
            return WebApiClient<PageOfOrder>.GetByFilter(string.Format("{0}/?{1}", Url, filter.GetFilter()));
        }

        public static int Synchro(PageOfOrder po, LoadBillInfo loadbill)
        {
            int sourceCount = 0;
            string ConfigPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NHibernate.config");
            IList<DBHelper.Entitys.WayBillInfo> sqlbatchparameter =new List<DBHelper.Entitys.WayBillInfo>();
            IList<DBHelper.Entitys.WayBillInfo> li = po.List;
            if (li.Count > 0)
            {
                using (IStatelessSession session = NHibernateSessionManager.Instance.GetNewSessionFrom(ConfigPath))
                {
                    //获取收货商与客户关系
                    Dictionary<int, string> CustomerIDs = DBHelper.Entitys.ExpressCurInfo.GetCustomers(li.Select(a => a.BusinessId).ToList<int>());
                    IList<DBHelper.Entitys.WayBillInfo> lisource = DBHelper.Entitys.WayBillInfo.GetByBussinessID(li.Select(a => a.OrderId).ToList<int>());
                    DateTime dt = DateTime.Now;
                    IEnumerable<DBHelper.Entitys.WayBillInfo> detailli = from cc in li where lisource.Where(a => a.OrderId == cc.OrderId).Count() == 0 select cc;
                    foreach (var item in detailli)
                    {
                        if (CustomerIDs.ContainsKey(item.BusinessId) && !string.IsNullOrEmpty(CustomerIDs[item.BusinessId]))
                        {
                            item.CustomerID = int.Parse(CustomerIDs[item.BusinessId]);
                        }
                        item.CreateTime = dt;
                        item.SenderProvince = "广州省";
                        item.LoadBillID = loadbill.ID;
                        item.LoadBillNO = loadbill.LoadBillNo;
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

        public static string GetbathInsertSql(IEnumerable<WayBillInfo> li)
        {
            StringBuilder sqlValue = new StringBuilder();
            string sqlHead = @"INSERT INTO WayBillInCome (ReceiverAddress,DeliveryID,CompanyId,CreateTime,ExpressNo,LoadBillID,BusinessID,ReceiverProvince,ReceiverName,Weight,OrderNo,SenderProvince,CustomerID,LoadBillNO) VALUES";
            foreach (var item in li)
            {
                sqlValue.AppendLine(string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}),", item.Address.ToSqlStr(), item.BusinessId, item.CompanyId.ToSqlStr(), item.CreateTime.ToDateStr(), item.ExpressNo.ToSqlStr(), item.LoadBillID, item.OrderId, item.Province.ToSqlStr(), item.Receiver.ToSqlStr(), item.Weight, item.OrderCode.ToSqlStr(), item.SenderProvince.ToSqlStr(), item.CustomerID.ToSqlStr(),item.LoadBillNO.ToSqlStr()));
            }
            return string.Format("{0} {1}", sqlHead, sqlValue.ToString().Substring(0, sqlValue.Length - 3));
        }
        #endregion

        #region entity method
        #endregion
    }

    public class PageOfOrder
    {
        public int Count{get;set;}
        public int PageIndex{get;set;}
        public int PageSize{get;set;}
        public List<WayBillInfo> List{get;set;}
    }

    public class Orderfilter
    {
        public int loadBillId{get;set;}
        public int pageindex{get;set;}
        public int pagesize { get; set; }
        public string GetFilter()
        {
            return string.Format("loadBillId={0}&pageindex={1}&pagesize={2}",loadBillId,pageindex,pagesize);
        }
    }
}

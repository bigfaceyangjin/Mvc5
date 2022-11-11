using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBHelper.Extend;
using System.Data;
using System.Net.Http;
using System.Collections;

namespace DBHelper.Entitys
{
    public class LoadBillInfo : BaseEntity<LoadBillInfo, int>
    {
        #region proprety
        /// <summary>
        /// 收货商ID
        /// </summary>
        public virtual int BusinessId { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public virtual int? CompanyId { get; set; }
        /// <summary>
        /// 报关时间
        /// </summary>
        public virtual DateTime? CustomsTime { get; set; }
        /// <summary>
        /// 提货单业务系统ID
        /// </summary>
        public virtual int LoadBillId { get; set; }
        /// <summary>
        /// 提货单号
        /// </summary>
        public virtual string LoadBillNo { get; set; }
        /// <summary>
        /// 提货单状态
        /// </summary>
        public virtual int Status { get; set; }
        /// <summary>
        /// 提货单重量
        /// </summary>
        public virtual decimal Weight { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public virtual int? CustomerID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 订单数
        /// </summary>
        public virtual int OrderCounts { get; set; }
        /// <summary>
        /// 提货费
        /// </summary>
        public virtual decimal LoadFee { get; set; }
        /// <summary>
        /// 预估成本提货费
        /// </summary>
        public virtual decimal PreTotalLoadFee { get; set; }
        /// <summary>
        /// 仓租费
        /// </summary>
        public virtual decimal StoreFee { get; set; }
        /// <summary>
        /// 操作费总额
        /// </summary>
        public virtual decimal TotalCollectFees { get; set; }
        /// <summary>
        /// 其他金额
        /// </summary>
        public virtual decimal OtherFee { get; set; }
        /// <summary>
        /// 应收总金额
        /// </summary>
        public virtual decimal TotalReceivableFee { get; set; }
        /// <summary>
        /// 是否已获取(0为未获取,1为获取)
        /// </summary>
        public virtual int IsGet { get; set; }
        #endregion

        #region common method
        /// <summary>
        /// 根据提货单业务系统主键ID获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static IList<LoadBillInfo> GetByBussinessID(IEnumerable<int> ids)
        {
            var query = NHibernateSession.CreateQuery("from LoadBillInfo where LoadBillId in (:ID)");
            query.SetParameterList("ID", ids);
            return query.List<LoadBillInfo>();
        }

        public static IList<LoadBillInfo> GetUnGetOrder()
        {
            var query = NHibernateSession.CreateQuery("from LoadBillInfo where IsGet=:status");
            query.SetParameter("status", 0);
            return query.List<LoadBillInfo>();
        }

        public static string SynchroOrder()
        {
            StringBuilder result = new StringBuilder();
            IList<LoadBillInfo> li = GetUnGetOrder();
            result.AppendLine(string.Format("获取{0}个提单", li.Count));
            foreach (var item in li)
            {
                int index = 1;
                int waybillCount = 0;
                PageOfOrder po;
                do
                {
                    po = WayBillInfo.GetFilterBySource(new Orderfilter() { loadBillId = item.LoadBillId, pagesize = 400, pageindex = index++ });
                    waybillCount += WayBillInfo.Synchro(po, item);
                }
                while (po.Count > po.PageIndex * po.PageSize);
                item.UpdateLoadBill(po.Count, po.Count == waybillCount ? 1 : 0);
                result.AppendLine(string.Format("提单号:{0},共获取{1}个运单", item.LoadBillNo, waybillCount));
            }
            return result.ToString();
        }

        public static string Synchro()
        {
            StringBuilder result = new StringBuilder();
            string ConfigPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NHibernate.config");
            List<DBHelper.Entitys.LoadBillInfo> sqlbatchparameter = new List<DBHelper.Entitys.LoadBillInfo>();
            List<DBHelper.Entitys.LoadBillInfo> li = DBHelper.Entitys.LoadBillInfo.GetAllBySource();
            result.AppendLine(string.Format("获取{0}个提单", li.Count));
            if (li.Count > 0)
            {
                Dictionary<string, decimal> loadFee = BaseFeeItem.GetB2CBillLading();
                using (IStatelessSession session = NHibernateSessionManager.Instance.GetNewSessionFrom(ConfigPath))
                {
                    Dictionary<int, string> CustomerIDs = DBHelper.Entitys.ExpressCurInfo.GetCustomers(li.Select(a => a.BusinessId).ToList<int>());//获取收货商与客户关系
                    IList<DBHelper.Entitys.LoadBillInfo> lisource = DBHelper.Entitys.LoadBillInfo.GetByBussinessID(li.Select(a => a.LoadBillId).ToList<int>());
                    IEnumerable<DBHelper.Entitys.LoadBillInfo> detailli = from cc in li where lisource.Where(a => a.LoadBillId == cc.LoadBillId).Count() == 0 select cc;
                    DateTime dt = DateTime.Now;
                    foreach (var item in detailli)
                    {
                        if (CustomerIDs.ContainsKey(item.BusinessId) && !string.IsNullOrEmpty(CustomerIDs[item.BusinessId]))
                        {
                            item.CustomerID = int.Parse(CustomerIDs[item.BusinessId]);
                        }
                        item.CreateTime = dt;
                        item.LoadFee = loadFee["DeliveryFee"] * item.Weight;//提货费
                        item.PreTotalLoadFee = loadFee["PreDeliveryFee"] * item.Weight;//预估成本提货费
                        item.IsGet = 0;//初始未获取运单标识
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
                                DBHelper.Entitys.LoadBillInfo.EditBySource(li.Select(o => o.LoadBillId).ToList());
                                tran.Commit();
                            }
                            result.AppendLine(string.Format("成功导入{0}个提单", li.Count));
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            return result.ToString();
        }

        public static string GetbathInsertSql(IEnumerable<LoadBillInfo> li)
        {
            StringBuilder sqlValue = new StringBuilder();
            string sqlHead = @"INSERT INTO LoadBillInCome(LoadBillNum,CustomerID,BusinessTime,CreateTime,CompanyID,DeliveryID,BillStatus,CompletionTime,BusinessID,BillWeight,LoadFee,PreTotalLoadFee) VALUES";
            foreach (var item in li)
            {
                sqlValue.AppendLine(string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}),", item.LoadBillNo.ToSqlStr(), item.CustomerID.ToSqlStr(), item.CustomsTime.ToDateStr(), item.CreateTime.ToDateStr(), item.CompanyId.ToSqlStr(), item.BusinessId, item.Status, item.CustomsTime.ToDateStr(), item.LoadBillId, item.Weight, item.LoadFee, item.PreTotalLoadFee));
            }
            return string.Format("{0} {1};{2};{3}", sqlHead, sqlValue.ToString().Substring(0, sqlValue.Length - 3), GetLoadBillFeSql(li), GetUpdateSql(li.Select(a => a.LoadBillId)));
        }

        public static string GetLoadBillFeSql(IEnumerable<LoadBillInfo> li)
        {
            string sql = @"INSERT INTO LoadBillInComeDetail(FeeType,Fee,FeeExplain,CreateTime,BillID) select 0, LoadFee,CONCAT('提单重量:',BillWeight,'计算提货费'),NOW(),ID from LoadBillInCome where LoadBillNum IN ('{0}')";
            return string.Format(sql, string.Join("','", li.Select(a => a.LoadBillNo)));
        }

        public static string GetUpdateSql(IEnumerable<int> loadBillID)
        {
            string sql = @"UPDATE LoadBillInCome a INNER JOIN (SELECT LoadBillID,SUM(Weight) AS OrderWeight FROM WayBillInCome WHERE LoadBillID IN ({0}) GROUP BY LoadBillID) b ON a.ID=b.LoadBillID
SET a.OrderWeight=b.OrderWeight WHERE a.ID IN ({0});";
            return string.Format(sql, string.Join(",", loadBillID));
        }
        #endregion

        #region entity method
        public virtual int UpdateLoadBill(int orderCount, int isGet)
        {
            string sql = string.Format(@"UPDATE LoadBillInCome a INNER JOIN (select LoadBillID,SUM(Weight) AS Weight FROM WayBillInCome where LoadBillID={0} GROUP BY LoadBillID) b
ON a.ID=b.LoadBillID
SET a.OrderWeight=b.Weight,a.OrderCounts={1},IsGet={2} where a.ID={0}", this.ID, orderCount, isGet);
            var query = NHibernateSession.CreateSQLQuery(sql);
            return query.ExecuteUpdate();
        }
        #endregion

        //----------------------根据提单ID同步 提单状态，清关时间，仓租--------------------
        /// <summary>
        /// 查询出需要同步数据的提单收入
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static IList<LoadBillInfo> GetLoadBillInComeTable()
        {
            var query = NHibernateSession.CreateQuery("from LoadBillInfo where Status<>:status");
            query.SetParameter("status", 7);
            return query.List<LoadBillInfo>();
            DataTable dt = new DataTable();
        }

        /// <summary>
        /// 同步数据操作优化新（一条一条去接口取速度慢）
        /// </summary>
        /// <returns></returns>
        public static int SynBillInComeNew()
        {
            int count = 0;
            IList<LoadBillInfo> li = GetLoadBillInComeTable();
            var url = System.Configuration.ConfigurationManager.AppSettings["SynBillInCome"].ToString();
            if (li.Count>0)
            {
                List<DBHelper.Entitys.BillWarehouseModel> apimodellist = WebApiClient<BillWarehouseModel>.GetList<BillWarehouseModel>(url, li.Select(a=>a.LoadBillId).ToList());
                List<BillWarehouseModel> dealli = new List<BillWarehouseModel>();
                foreach (var item in apimodellist)
                {
                    var model = li.First(a=>a.LoadBillId==item.BusinessID);
                    if (model!=null)
                    {
                        var modelss = BillWarehouseModel.GetById(model.ID);

                        modelss.Status = item.Status > 0 ? item.Status : modelss.Status;
                        modelss.CustomsTime = item.CustomsTime.HasValue ? item.CustomsTime.Value : modelss.CustomsTime;
                        modelss.Fee = item.Fee > 0 ? item.Fee : modelss.Fee;
                        modelss.LoadTime = item.LoadTime.HasValue ? item.LoadTime.Value : modelss.LoadTime;
                        modelss.DeliveryTime = item.DeliveryTime.HasValue ? item.DeliveryTime : modelss.DeliveryTime;
                        modelss.StorageFeeReason = !string.IsNullOrEmpty(item.StorageFeeReason) ? item.StorageFeeReason : modelss.StorageFeeReason;
                        dealli.Add(modelss);
                    }
                }
                if (dealli.Count>0)
                {
                    using (IStatelessSession s = NHibernateSession.SessionFactory.OpenStatelessSession())
                    {
                        var tran = s.BeginTransaction();
                        try
                        {
                            foreach (var item in dealli)
                            {
                                s.Update(item);
                                count++;
                            }
                            tran.Commit();
                            NHibernateSession.Clear();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                        finally
                        {
                            s.Dispose();
                        }
                    }
                }
            }
            return count;
        }
    }

    /// <summary>
    /// 仓租信息
    /// </summary>
    public class BillWarehouseModel : BaseEntity<BillWarehouseModel, int>
    {
        public virtual int BusinessID { get; set; } //业务ID
        public virtual DateTime? CustomsTime { get; set; } //清关时间
        public virtual decimal Fee { get; set; }          //仓租
        public virtual int Status { get; set; }           //提单状态

        public virtual DateTime? LoadTime { get; set; }      //到货日期
        public virtual DateTime? DeliveryTime { get; set; }  //提货日期
        public virtual string StorageFeeReason { get; set; } //仓租原因

    }

    public class LoadBillInComeDetail : BaseEntity<LoadBillInComeDetail, int>
    {
        #region property
        /// <summary>
        /// 费用类型
        /// </summary>
        public virtual int FeeType { get; set; }
        /// <summary>
        /// 费用值
        /// </summary>
        public virtual decimal Fee { get; set; }
        /// <summary>
        /// 费用简介
        /// </summary>
        public virtual string FeeExplain { get; set; }
        /// <summary>
        /// 业务系统备注
        /// </summary>
        public virtual string Explain { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 提货单号
        /// </summary>
        public virtual int BillID { get; set; }
        /// <summary>
        /// 提货单业务系统ID
        /// </summary>
        public virtual int LoadBillId { get; set; }
        /// <summary>
        /// 业务系统主键ID
        /// </summary>
        public virtual int DetailId { get; set; }
        #endregion

        #region common method
        /// <summary>
        /// 根据提货单业务系统主键ID获取数据
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public static IList<LoadBillInComeDetail> GetByBussinessID(IEnumerable<int> Ids)
        {
            var query = NHibernateSession.CreateQuery("from LoadBillInComeDetail where DetailId in (:ID)");
            query.SetParameterList("ID", Ids);
            return query.List<LoadBillInComeDetail>();
        }

        public static string Synchro()
        {
            StringBuilder result = new StringBuilder();
            string ConfigPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NHibernate.config");
            List<DBHelper.Entitys.LoadBillInComeDetail> sqlbatchparameter = new List<DBHelper.Entitys.LoadBillInComeDetail>();
            List<DBHelper.Entitys.LoadBillInComeDetail> li = DBHelper.Entitys.LoadBillInComeDetail.GetAllBySource();
            result.AppendLine(string.Format("成功获取{0}个提单费用", li.Count));
            if (li.Count > 0)
            {
                IStatelessSession session = NHibernateSessionManager.Instance.GetNewSessionFrom(ConfigPath);
                using (var tran = session.BeginTransaction())
                {
                    try
                    {
                        IList<DBHelper.Entitys.LoadBillInComeDetail> lisource = DBHelper.Entitys.LoadBillInComeDetail.GetByBussinessID(li.Select(a => a.DetailId));
                        IEnumerable<DBHelper.Entitys.LoadBillInComeDetail> detailli = from cc in li where lisource.Where(a => a.LoadBillId == cc.LoadBillId).Count() == 0 select cc;
                        IEnumerable<DBHelper.Entitys.LoadBillInfo> lb = DBHelper.Entitys.LoadBillInfo.GetByBussinessID(detailli.Select(a => a.LoadBillId));
                        DateTime dt = DateTime.Now;
                        foreach (var item in detailli)
                        {
                            if (item.FeeType == 0)
                            {
                                item.CreateTime = dt;
                                item.ToStoreFee(lb.Where(a => a.LoadBillId == item.LoadBillId).First().ID);
                                sqlbatchparameter.Add(item);
                            }
                        }
                        if (sqlbatchparameter.Count() > 0)
                        {
                            session.CreateSQLQuery(GetbathInsertSql(sqlbatchparameter)).ExecuteUpdate();
                        }
                        tran.Commit();
                        DBHelper.Entitys.LoadBillInComeDetail.EditBySource(li.Select(o => o.DetailId).ToList());
                        result.AppendLine(string.Format("成功导入{0}个提单费用", li.Count));
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// 批量插入语句
        /// </summary>
        /// <returns></returns>
        public static string GetbathInsertSql(IEnumerable<LoadBillInComeDetail> li)
        {
            StringBuilder sqlValue = new StringBuilder();
            string sqlHead = @"INSERT INTO LoadBillInComeDetail(FeeType,Fee,FeeExplain,CreateTime,BillID,BussinessID) VALUES";
            foreach (var item in li)
            {
                sqlValue.AppendLine(string.Format("({0},{1},{2},{3},{4},{5}),", item.FeeType, item.Fee, item.FeeExplain.ToSqlStr(), item.CreateTime.ToDateStr(), item.BillID, item.DetailId));
            }
            return string.Format("{0} {1};{2}", sqlHead, sqlValue.ToString().Substring(0, sqlValue.Length - 3), GetStoreFeeSql(li));
        }

        /// <summary>
        /// 更新提货单的仓租
        /// </summary>
        /// <param name="li"></param>
        /// <returns></returns>
        public static string GetStoreFeeSql(IEnumerable<LoadBillInComeDetail> li)
        {
            string sql = @"UPDATE LoadBillInCome a INNER JOIN LoadBillInComeDetail b ON a.ID=b.BillID SET a.StoreFee=b.Fee where b.FeeType=1 AND b.BussinessID IN ({0});";
            return string.Format(sql, string.Join(",", li.Select(a => a.DetailId)));
        }
        #endregion

        #region entity common
        public virtual void ToStoreFee(int billID)
        {
            if (!string.IsNullOrEmpty(Explain))
            {
                FeeExplain = string.Format("仓租,仓租原因:{0}", Explain);
            }
            else
            {
                FeeExplain = "仓租";
            }
            FeeType = 1;
            BillID = billID;
        }
        #endregion
    }
}





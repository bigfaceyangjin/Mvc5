using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Reconciliation;
using Core.Reconciliation.Repositories;
using ProjectBase.Data;
using ProjectBase.Utils.Entities;
using NHibernate;
using System.Data;

namespace Data.Reconciliation
{
    public class UnusualLoadBillRepository : AbstractNHibernateDao<UnusualLoadBill, int>, IUnusualLoadBillRepository
    {
        public long GetUnusualLoadBillCount(ParameterFilter filter)
        {
            string sql = @" FROM LoadBillInCome a RIGHT JOIN LoadBillCost b ON a.LoadBillNum=b.LoadBillNum LEFT JOIN CustomerInfo c ON a.CustomerID=c.ID WHERE b.`Status`!=0";
            if (filter.HasQueryString)
                sql = filter.ToHql();
            else
                sql += filter.ToHql();

            var paras = filter.GetParameters();
            var countQuery = NHibernateSession.CreateSQLQuery(string.Format("select COUNT(b.ID) as Count {0}", sql));
            foreach (var key in paras.Keys)
            {
                countQuery.SetParameter(key, paras[key]);
            }
            int pageIndex = filter.PageIndex;
            int pageSize = filter.PageSize;
            var Count = countQuery.UniqueResult<long>();
            return Count;
        }
        public List<long> GetUnusualLoadBillCount()
        {
            List<long> result = new List<long>();
            using (ISession s = NHibernateSession.SessionFactory.OpenSession())
            {
                string sql = @"select COUNT(b.ID) as Count FROM LoadBillCost b WHERE b.`Status`!=0;
               select count(t.WayBillCostID) from ExpressNoExceptionDetail t ;";
                IDbCommand com = s.Connection.CreateCommand();
                com.CommandText = sql;
                IDataReader dr = com.ExecuteReader();
                do
                {
                    while (dr.Read())
                    {
                        result.Add(long.Parse(dr.GetValue(0).ToString()));
                    }
                } while (dr.NextResult());
                return result;
            }
        }
        public IPageOfList<UnusualLoadBill> GetUnusualByFilter(ParameterFilter filter)
        {
            string column = @"b.ID,
b.ReconcileDate AS ReconcileDate,
c.Cus_Name AS CusName,
b.LoadBillNum AS LoadBillNum,
b.FeeWeight AS FeeWeight,
(SELECT SUM(Weight)/1000 FROM WayBillCost WHERE BatchNO=b.LoadBillNum) AS ExpressWeight,
(SELECT cast(COUNT(*) as signed) FROM WayBillCost where BatchNO=b.LoadBillNum) AS ExpressCount,
a.CompletionTime AS CompletionTime,
IFNULL(b.GroundHandlingFee,0) AS GroundHandlingFee,
IFNULL(b.StoreFee,0) AS CostStoreFee,
CASE WHEN b.ID IS NULL THEN a.PreTotalCollectFees ELSE 0.0 END AS CostExpressFee,
CASE WHEN b.ID IS NULL THEN a.PreTotalOperateFee ELSE 0.0 END AS CostOperateFee,
null AS CostOtherFee,
null AS CostTotalFee,
b.PayStatus AS CostStatus,
a.LoadFee AS InComeLoadFee,
a.StoreFee AS InComeStoreFee,
a.TotalCollectFees AS InComeExpressFee,
a.TotalOperateFee AS InComeOperateFee,
a.OtherFee AS InComeOtherFee,
null AS InComeTotalFee,
a.PayStatus AS InComeStatus,
null AS TotalGrossProfit,
null AS GrossProfitRate,
b.`Status`";
            string sql = @" FROM LoadBillInCome a RIGHT JOIN LoadBillCost b ON a.LoadBillNum=b.LoadBillNum LEFT JOIN CustomerInfo c ON a.CustomerID=c.ID WHERE b.`Status`!=0";
            if (filter.HasQueryString)
                sql = filter.ToHql();
            else
                sql += filter.ToHql();

            var paras = filter.GetParameters();
            var countQuery = NHibernateSession.CreateSQLQuery(string.Format("select COUNT(b.ID) as Count {0}", sql));
            var query = NHibernateSession.CreateSQLQuery(string.Format("select {0} {1} {2} ", column, sql, filter.GetOrderString()));
            foreach (var key in paras.Keys)
            {
                countQuery.SetParameter(key, paras[key]);
                query.SetParameter(key, paras[key]);
            }
            int pageIndex = filter.PageIndex;
            int pageSize = filter.PageSize;
            var Count = countQuery.UniqueResult<long>();
            var list = query.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean(typeof(UnusualLoadBill))).SetFirstResult(pageIndex * pageSize).SetMaxResults(pageSize).List<UnusualLoadBill>().ToList();
            return new LRPageOfList<UnusualLoadBill>(list, pageIndex, pageSize, Count);
        }
    }

    public class LoadBillCostRepository : AbstractNHibernateDao<LoadBillCost, int>, ILoadBillCostRepository
    {
        public IList<LoadBillCost> GetByLoadBillNum(string loadBillNum)
        {
            var query = NHibernateSession.CreateQuery("from LoadBillCost where LoadBillNum=:loadBillNum");
            query.SetParameter("loadBillNum", loadBillNum);
            return query.List<LoadBillCost>();
        }
    }
}

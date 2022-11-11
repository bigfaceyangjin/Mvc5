using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightSynchro.Entitys
{
    public class LoadBillInCome : BaseEntity<LoadBillInCome, int>
    {
        /// <summary>
        /// 提货单号
        /// </summary>
        public virtual string LoadBillNum { get; set; }
        /// <summary>
        /// 运费总额
        /// </summary>
        public virtual decimal TotalCollectFees { get; set; }
        /// <summary>
        /// 操作费总额
        /// </summary>
        public virtual decimal TotalOperateFee { get; set; }

        public static void UpdateFee(IEnumerable<int> ids)
        {
            string sql = @"UPDATE LoadBillInCome a INNER JOIN 
(SELECT LoadBillID,SUM(ExpressFee) AS TotalExpressFee,SUM(OperateFee) AS TotalOperateFee,
SUM(PreExpressFee) AS TotalPreExpressFee, SUM(PreOperateFee) AS TotalPreOperateFee 
FROM WayBillInCome where LoadBillID in (:ids) GROUP BY LoadBillID) AS b
ON a.ID = b.LoadBillID
SET a.TotalCollectFees = b.TotalExpressFee, 
a.TotalOperateFee = b.TotalOperateFee,
a.PreTotalCollectFees = b.TotalPreExpressFee,
a.PreTotalOperateFee = b.TotalPreOperateFee
where a.id in (:ids)";
            var query = NHibernateSession.CreateSQLQuery(sql);
            query.SetParameterList("ids", ids);
            query.ExecuteUpdate();
        }
    }
}

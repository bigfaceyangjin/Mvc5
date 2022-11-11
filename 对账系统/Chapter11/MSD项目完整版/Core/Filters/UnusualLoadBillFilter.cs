using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Filters
{
    public class UnusualLoadBillFilter : ParameterFilter
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CusName { get; set; }
        /// <summary>
        /// 异常提单号
        /// </summary>
        public string UnusualLoadBillNum { get; set; }
        /// <summary>
        /// 清关开始日期
        /// </summary>
        public DateTime? CompletionSTime { get; set; }
        /// <summary>
        /// 清关结束日期
        /// </summary>
        public DateTime? CompletionETime { get; set; }
        /// <summary>
        /// 成本异常状态
        /// </summary>
        public LoadBillExceptionTypeEnum? Status { get; set; }
        /// <summary>
        /// 对账开始日期
        /// </summary>
        public virtual DateTime? ReconcileSTime { get; set; }
        /// <summary>
        /// 对账结束日期
        /// </summary>
        public virtual DateTime? ReconcileETime { get; set; }
        /// <summary>
        /// 构造查询参数
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetParameters()
        {
            var result = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(CusName))
            {
                result["CusName"] = string.Format("{0}%", CusName.Trim());
            }
            if (!string.IsNullOrEmpty(UnusualLoadBillNum))
            {
                result["UnusualLoadBillNum"] = UnusualLoadBillNum.Trim();
            }
            if (CompletionSTime.HasValue)
            {
                result["CompletionSTime"] = CompletionSTime.Value;
            }
            if (CompletionETime.HasValue)
            {
                result["CompletionETime"] = CompletionETime.Value;
            }
            if (Status.HasValue)
            {
                result["Status"] = Status.Value;
            }
            if (ReconcileSTime.HasValue)
            {
                result["ReconcileSTime"] = ReconcileSTime.Value;
            }
            if (ReconcileETime.HasValue)
            {
                result["ReconcileETime"] = ReconcileETime.Value;
            }
            return result;
        }

        /// <summary>
        /// 生产NHQL查询语句
        /// </summary>
        /// <returns></returns>
        public override string ToHql()
        {
            string hql = "";
            if (!string.IsNullOrEmpty(CusName))
            {
                hql += " and c.Cus_Name like :CusName";
            }
            if (!string.IsNullOrEmpty(UnusualLoadBillNum))
            {
                hql += " and b.LoadBillNum =:UnusualLoadBillNum";
            }
            if (CompletionSTime.HasValue)
            {
                hql += " and a.CompletionTime >=:CompletionSTime";
            }
            if (CompletionETime.HasValue)
            {
                hql += " and a.CompletionTime <=:CompletionETime";
            }
            if (Status.HasValue)
            {
                hql += " and b.Status=:Status";
            }
            if (ReconcileSTime.HasValue)
            {
                hql += " and b.ReconcileDate>=:ReconcileSTime";
            }
            if (ReconcileETime.HasValue)
            {
                hql += " and b.ReconcileDate<=:ReconcileETime";
            }
            return hql;
        }
    }
}

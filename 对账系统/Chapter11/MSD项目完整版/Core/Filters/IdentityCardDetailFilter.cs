using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CostFlow;

namespace Core.Filters
{
    /// <summary>
    /// 身份证验证流水查询
    /// </summary>
    public class IdentityCardDetailFilter : ParameterFilter
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string Cus_Name { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public virtual string IdentityNmber { get; set; }

        public virtual DateTime? ValideTime { get; set; }

        public virtual DateTime? ValideTimeTo { get; set; }
        /// <summary>
        /// 结算状态
        /// </summary>
        public virtual Core.PayStatus? Status { get; set; }

        /// <summary>
        /// 构造查询参数
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetParameters()
        {
            var result = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Cus_Name))
            {
                result["CusName"] = Cus_Name.Trim();
            }
            if (!string.IsNullOrEmpty(IdentityNmber))
            {
                result["IdentityNmber"] = IdentityNmber.Trim();
            }
            if (Status != null)
            {
                result["Status"] =Status.Value;
            }
            if (ValideTime != null && ValideTimeTo != null)
            {
                result["ValideTime"] = ValideTime.Value;
                result["ValideTimeTo"] = ValideTimeTo.Value;
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
            if (!string.IsNullOrEmpty(Cus_Name))
            {
                hql += " and CustomerInfoBy.CusName =:CusName ";
            }
            if (!string.IsNullOrEmpty(IdentityNmber))
            {
                hql += " and IdentityNmber =:IdentityNmber ";
            }
            if (Status != null)
            {
                hql += " and Status= :Status";
            }
            if (ValideTime != null && ValideTimeTo != null)
            {
                //hql += " AND CreateTime BETWEEN :DateFrom AND :DateTo ";
                hql += " AND ValideTime>= :ValideTime AND ValideTime <= :ValideTimeTo ";
            }
            return hql;
        }
    }
}

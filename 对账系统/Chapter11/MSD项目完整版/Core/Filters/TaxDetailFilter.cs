using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Filters
{
    public class TaxDetailFilter : ParameterFilter
    {
        /// <summary>
        /// 客户简称
        /// </summary>
        public string CusShortName{get;set;}
        /// <summary>
        /// 提单号
        /// </summary>
        public string LoadBillNum{get;set;}
        /// <summary>
        /// 运单号
        /// </summary>
        public string ExpressNo{get;set;}
        /// <summary>
        /// 结算状态
        /// </summary>
        public Core.PayStatus? PayStatus{get;set;}
        /// <summary>
        /// 构造查询参数
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetParameters()
        {
            var result = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(CusShortName))
            {
                result["CusShortName"] = CusShortName.Trim();
            }
            if (!string.IsNullOrEmpty(LoadBillNum))
            {
                result["LoadBillNum"] = LoadBillNum.Trim();
            }
            if (!string.IsNullOrEmpty(ExpressNo))
            {
                result["ExpressNo"] = ExpressNo.Trim();
            }
            if (PayStatus.HasValue)
            {
                result["PayStatus"] = PayStatus.Value;
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
            if (!string.IsNullOrEmpty(CusShortName))
            {
                hql += " and CustomerInfoBy.CusName =:CusShortName ";
            }
            if (!string.IsNullOrEmpty(LoadBillNum))
            {
                hql += " and LoadBillNum =:LoadBillNum ";
            }
            if (!string.IsNullOrEmpty(ExpressNo))
            {
                hql += " and ExpressNo =:ExpressNo ";
            }
            if (PayStatus.HasValue)
            {
                hql += " and PayStatus =:PayStatus ";
            }
            return hql;
        }
    }
}

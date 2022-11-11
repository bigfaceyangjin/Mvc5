using Core.Receivable;
using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Filters
{
    public class WayBillInComeFilter:ParameterFilter
    {
        /// <summary>
        /// 运单号
        /// </summary>
        public string ExpressNo { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 提单号
        /// </summary>
        public string LoadBillNum { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public PayStatus? PayStatus { get; set; }
        /// <summary>
        /// 运单收入类型
        /// </summary>
        public ExpressType? ExpressType { get; set; }
        /// <summary>
        /// 快递类型
        /// </summary>
        public WayBillType? WayBillType { get; set; }

        /// <summary>
        /// 构造查询参数
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetParameters()
        {
            var result = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(ExpressNo))
            {
                result["ExpressNo"] = ExpressNo.Trim();
            }
            if (!string.IsNullOrEmpty(OrderNo))
            {
                result["OrderNo"] = OrderNo.Trim();
            }
            if (!string.IsNullOrEmpty(CustomerName))
            {
                result["CustomerName"] = CustomerName.Trim();
            }
            if (!string.IsNullOrEmpty(LoadBillNum))
            {
                result["LoadBillNum"] = LoadBillNum.Trim();
            }
            if (PayStatus.HasValue)
            {
                result["PayStatus"] = PayStatus.Value;
            }
            if (ExpressType.HasValue)
            {
                result["ExpressType"] = ExpressType.Value;
            }
            if (WayBillType.HasValue)
            {
                result["WayBillType"] = WayBillType.Value;
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
            if (!string.IsNullOrEmpty(ExpressNo))
            {
                hql += " and ExpressNo =:ExpressNo ";
            }
            if (!string.IsNullOrEmpty(OrderNo))
            {
                hql += " and OrderNo =:OrderNo ";
            }
            if (!string.IsNullOrEmpty(CustomerName))
            {
                hql += " and CustomerInfoBy.CusName =:CustomerName ";
            }
            if (!string.IsNullOrEmpty(LoadBillNum))
            {
                hql += " and LoadBillBy.LoadBillNum =:LoadBillNum ";
            }
            if (PayStatus.HasValue)
            {
                hql += " and PayStatus =:PayStatus ";
            }
            if (ExpressType.HasValue)
            {
                hql += " and ExpressType =:ExpressType ";
            }
            if (WayBillType.HasValue)
            {
                hql += " and WayBillType =:WayBillType ";
            }
            return hql;
        }
    }
}

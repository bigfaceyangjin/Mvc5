using Core.CostFlow.Repositories;
using Core.Customer;
using Core.Filters;
using ProjectBase.Data;
using ProjectBase.Utils.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CostFlow
{
    /// <summary>
    /// 身份证验证流水
    /// </summary>
    public class IdentityCardDetail : DomainObject<IdentityCardDetail, int, IIdentityCardDetailRepository>
    {
        /// <summary>
        /// 收货商ID
        /// </summary>
        //public virtual int DeliveryID { get; set; }
        public virtual ExpressCurInfo ExpressCurInfoBy { get; set; }
        /// <summary>
        /// 身份证验证费
        /// </summary>
        public virtual decimal Fee { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public virtual string ExpressNo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 验证时间
        /// </summary>
        public virtual DateTime ValideTime { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderID { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public virtual string IdentityNmber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string IdentityName { get; set; }
        /// <summary>
        /// 结算状态
        /// </summary>
        public virtual PayStatus Status { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        //public virtual int CustomerID { get; set; }
        public virtual CustomerInfo CustomerInfoBy { get; set; }

   

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IPageOfList<IdentityCardDetail> GetByFilter(IdentityCardDetailFilter filter)
        {
            return Dao.GetByFilter(filter);
        }
    }
}

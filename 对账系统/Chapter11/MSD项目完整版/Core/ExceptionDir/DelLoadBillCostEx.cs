using Core.ExceptionDir.Repositories;
using ProjectBase.Data;
/* ==============================================================================
   * 功能描述：DelLoadBillCostEx  
   * 创 建 者：Zouqj
   * 创建日期：2015/6/26 11:06:39
   ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ExceptionDir
{
    public class DelLoadBillCostEx : DomainObject<DelLoadBillCostEx, int, IDelLoadBillCostExRepository>
    {
        #region property
        /// <summary>
        /// 提单号
        /// </summary>
        public virtual string LoadBillNum { get; set; }
        /// <summary>
        /// 提货日期
        /// </summary>
        public virtual DateTime LoadTime { get; set; }
        /// <summary>
        /// 地勤费
        /// </summary>
        public virtual decimal GroundHandlingFee { get; set; }
        /// <summary>
        /// 仓租
        /// </summary>
        public virtual decimal StoreFee { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 到货时间
        /// </summary>
        public virtual DateTime DeliveryTime { get; set; }
        /// <summary>
        /// 实重
        /// </summary>
        public virtual decimal FactWeight { get; set; }
        /// <summary>
        /// 计费重量(kg)
        /// </summary>
        public virtual decimal FeeWeight { get; set; }
        /// <summary>
        /// 总成本
        /// </summary>
        public virtual decimal TotalFee { get; set; }
        /// <summary>
        /// 产生仓租原因
        /// </summary>
        public virtual string StorageFeeReason { get; set; }
        /// <summary>
        /// 结算月份
        /// </summary>
        public virtual DateTime ReconcileDate { get; set; }
        /// <summary>
        /// 结算状态
        /// </summary>
        public virtual Core.PayStatus PayStatus { get; set; }
        /// <summary>
        /// 结算时间
        /// </summary>
        public virtual DateTime PayTime { get; set; }
        /// <summary>
        /// 异常状态
        /// </summary>
        public virtual LoadBillExceptionTypeEnum Status { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public virtual DateTime DelTime { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public virtual int CustomerID { get; set; }

        /// <summary>
        /// 其他费用
        /// </summary>
        public virtual decimal TotalOtherFee { get; set; }
        #endregion
    }
}

using Core.ExceptionDir.Repositories;
using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ExceptionDir
{
    public class DelWayBillCostEx : DomainObject<DelWayBillCostEx, int, IDelWayBillCostExRepository>
    {
        #region property
        /// <summary>
        /// 邮件号
        /// </summary>
        public virtual string ExpressNo{get;set;}
        /// <summary>
        /// 邮资
        /// </summary>
        public virtual decimal WayBillFee{get;set;}
        /// <summary>
        /// 重量
        /// </summary>
        public virtual decimal Weight{get;set;}
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime{get;set;}
        /// <summary>
        /// 寄达地
        /// </summary>
        public virtual string SendAddress{get;set;}
        /// <summary>
        /// 邮件处理费
        /// </summary>
        public virtual decimal ProcessingFee{get;set;}
        /// <summary>
        /// 产品
        /// </summary>
        public virtual string Product{get;set;}
        /// <summary>
        /// 批次
        /// </summary>
        public virtual string BatchNO{get;set;}
        /// <summary>
        /// 收寄日期
        /// </summary>
        public virtual DateTime PostingTime{get;set;}
        /// <summary>
        /// 结算月份
        /// </summary>
        public virtual DateTime ReconcileDate{get;set;}
        /// <summary>
        /// 删除时间
        /// </summary>
        public virtual DateTime DelTime{get;set;}
        #endregion
    }
}

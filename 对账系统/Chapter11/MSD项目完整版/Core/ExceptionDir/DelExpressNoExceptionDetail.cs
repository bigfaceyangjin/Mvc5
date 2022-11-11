using Core.ExceptionDir.Repositories;
using ProjectBase.Data;
/* ==============================================================================
   * 功能描述：DelExpressNoExceptionDetail  
   * 创 建 者：Zouqj
   * 创建日期：2015/6/26 11:11:19
   ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ExceptionDir
{
    public class DelExpressNoExceptionDetail : DomainObject<DelExpressNoExceptionDetail, int, IDelExpressNoExceptionDetailRepository>
    {
        #region property
        /// <summary>
        /// 运单号
        /// </summary>
        public virtual string ExpressNo{get;set;}
        /// <summary>
        /// 异常类型
        /// </summary>
        public virtual WayBillExceptionTypeEnum ExceptionType{get;set;}
        /// <summary>
        /// 异常说明
        /// </summary>
        public virtual string ExceptionMsg{get;set;}
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime{get;set;}
        /// <summary>
        /// 提单号
        /// </summary>
        public virtual string LoadBillNO{get;set;}
        /// <summary>
        /// 对应删除运单成本对象
        /// </summary>
        public virtual DelWayBillCostEx DelWayBillCostExBy{get;set;}
        /// <summary>
        /// 收寄日期
        /// </summary>
        public virtual DateTime PostingTime{get;set;}
        /// <summary>
        /// 删除时间
        /// </summary>
        public virtual DateTime DelTime{get;set;}
        #endregion
    }
}

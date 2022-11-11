using Core.Receivable.Repositories;
using ProjectBase.Data;
using ProjectBase.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Receivable
{
    /// <summary>
    /// 税费流水
    /// </summary>
    public class TaxDetail : DomainObject<TaxDetail, int, ITaxDetailRepository>
    {
        #region property
        /// <summary>
        /// 运单号
        /// </summary>
        public virtual string ExpressNo{get;set;}
        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderNO{get;set;}
        /// <summary>
        /// 提单号
        /// </summary>
        public virtual string LoadBillNum{get;set;}
        /// <summary>
        /// 税款
        /// </summary>
        public virtual decimal TaxFee{get;set;}
        /// <summary>
        /// 税费确认时间
        /// </summary>
        public virtual DateTime AffirmTime{get;set;}
        /// <summary>
        /// 税单号
        /// </summary>
        public virtual string TaxBillNO{get;set;}
        /// <summary>
        /// 税费导入时间
        /// </summary>
        public virtual DateTime InputTime{get;set;}
        /// <summary>
        /// 对应客户信息
        /// </summary>
        public virtual Customer.CustomerInfo CustomerInfoBy{get;set;}
        /// <summary>
        /// 结算状态
        /// </summary>
        public virtual Core.PayStatus PayStatus{get;set; }
        #endregion

        #region common method
        public static IPageOfList<TaxDetail> GetByFilter(ParameterFilter filter)
        {
            return Dao.GetByFilter(filter);
        }
        #endregion
    }
}

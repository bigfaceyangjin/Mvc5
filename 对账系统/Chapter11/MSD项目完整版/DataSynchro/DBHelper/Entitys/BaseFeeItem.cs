using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBHelper.Extend;

namespace DBHelper.Entitys
{
    public class BaseFeeItem : BaseEntity<BaseFeeItem, int>
    {
        #region property
        /// <summary>
        /// 费用名
        /// </summary>
        public virtual string FullName{get;set;}
        /// <summary>
        /// 费用项代码
        /// </summary>
        public virtual string Code{get;set;}
        /// <summary>
        /// 费用
        /// </summary>
        public virtual decimal Fee{get;set;}
        /// <summary>
        /// 业务类型,默认0快件
        /// </summary>
        public virtual int BusinessType { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public virtual string CreateUser{get;set;}
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime? CreateTime{get;set;}
        /// <summary>
        /// 删除标识,默认0未删除
        /// </summary>
        public virtual int VoidFlag { get; set; }
        #endregion

        #region common method
        /// <summary>
        /// 获取快件提货费单价和预估提货费单价
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string,decimal> GetB2CBillLading()
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            var query = NHibernateSession.CreateSQLQuery("select `Code`,Fee FROM Base_FeeItem WHERE `Code` IN ('PreDeliveryFee','DeliveryFee') AND BusinessType=0;");
            foreach (object[] item in query.List<object[]>())
            {
                result.Add(item[0].ToString(),decimal.Parse(item[1].ToString()));
            }
            return result;
        }
        #endregion
    }
}

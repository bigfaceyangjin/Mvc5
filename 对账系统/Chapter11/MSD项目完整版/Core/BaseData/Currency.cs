using Core.BaseData.Repositories;
using Core.Filters;
using ProjectBase.Data;
using ProjectBase.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BaseData
{
    /// <summary>
    /// 币种
    /// </summary>
    public class Currency : DomainObject<Currency, int, ICurrencyRepository>
    {
        //全称
        public virtual string FullName { get; set; }
        //简称
        public virtual string ShortName { get; set; }
        //汇率
        public virtual decimal ExchangeRate { get; set; }
        //符号
        public virtual string Symbol { get; set; }
        //创建用户
        public virtual int CreateUser { get; set; }
        //创建时间
        public virtual DateTime CreateTime { get; set; }
        //是否有效
        public virtual int VoidFlag { get; set; }
        //是否默认
        public virtual int IsDefault { get; set; }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IPageOfList<Currency> GetByFilter(CurrencyFilter filter)
        {
            return Dao.GetByFilter(filter);
        }
      
    }
}

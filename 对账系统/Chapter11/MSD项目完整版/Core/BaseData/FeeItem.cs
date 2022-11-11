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
    /// 费用项
    /// </summary>
    public class FeeItem : DomainObject<FeeItem, int, IFeeItemRepository>
    {
        //全称
        public virtual string FullName { get; set; }
        //简称
        public virtual string ShortName { get; set; }
        //创建用户
        public virtual string CreateUser { get; set; }
        //创建时间
        public virtual string CreateTime { get; set; }
        //是否有效
        public virtual int VoidFlag { get; set; }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IPageOfList<FeeItem> GetByFilter(FeeItemFilter filter)
        {
            return Dao.GetByFilter(filter);
        }

    }
}

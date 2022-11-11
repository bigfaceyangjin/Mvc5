using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Filters
{
    public class FeeItemFilter : ParameterFilter
    {
        /// <summary>
        /// 全称
        /// </summary>
        public virtual string FullName { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public virtual string ShortName { get; set; }

      

        /// <summary>
        /// 构造查询参数
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetParameters()
        {
            var result = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(FullName))
            {
                result["FullName"] = FullName.Trim();
            }
            if (!string.IsNullOrEmpty(ShortName))
            {
                result["ShortName"] = ShortName.Trim();
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
            if (!string.IsNullOrEmpty(FullName))
            {
                hql += " and FullName =:FullName ";
            }
            if (!string.IsNullOrEmpty(ShortName))
            {
                hql += " and ShortName =:ShortName ";
            }       
            return hql;
        }
    }
}

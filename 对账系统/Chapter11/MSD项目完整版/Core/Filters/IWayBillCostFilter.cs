/* ==============================================================================
   * 功能描述：IWayBillCostFilter  
   * 创 建 者：Zouqj
   * 创建日期：2015/6/3 13:24:53
   ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProjectBase.Data;

namespace Core.Filters
{
   public class IWayBillCostFilter : ParameterFilter
    {
       
        /// <summary>
        /// 构造查询参数
        /// </summary>
        /// <returns></returns>
       public override Dictionary<string, object> GetParameters()
       {
           var result = new Dictionary<string, object>();
           return result;
       }

        /// <summary>
        /// 生产NHQL查询语句
        /// </summary>
        /// <returns></returns>
       public override string ToHql()
       {
          return string.Empty;
       }
    }
}

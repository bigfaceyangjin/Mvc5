using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ProjectBase.Utils
{
    public static class HtmlExpand
    {        

        /// <summary>
        /// 把枚举变量转换为SelectListItem的集合
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="obj">枚举的type</param>
        /// <param name="addall">是否增加所有选项</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> EnumToList(this HtmlHelper helper, Type obj, bool addall = true)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            if (addall)
            {
                result.Add(new SelectListItem { Text = "所有", Value = "" });
            }
            foreach (string item in Enum.GetNames(obj))
            {
                result.Add(new SelectListItem { Text = Enum.Parse(obj, item).GetDescription(false), Value = ((int)Enum.Parse(obj, item)).ToString() });
            }
            return result;
        }

        /// <summary>
        /// 格式化decimal 保留2位小数
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static decimal FormatDeciaml(decimal d)
        {
            return decimal.Round(d, 2);
        }

        /// <summary>
        /// 格式化decimal 保留2位小数
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static decimal FormatDeciaml(decimal? d)
        {
            return decimal.Round(d.Value, 2);
        }
    }
}

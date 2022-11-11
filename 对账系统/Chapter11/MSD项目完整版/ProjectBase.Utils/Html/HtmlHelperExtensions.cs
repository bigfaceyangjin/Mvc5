using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;
using ProjectBase.Utils.Entities;

namespace ProjectBase.Utils.Web
{
    public static class HtmlHelperExtensions
    {
        private const string SubmitName = "submit";

        public static string Link(this UrlHelper helper,string linkText,string action,string controller)
        {
            return string.Format("<a href=\"{0}\" class=\"sright\"><b class=\"by\">{1}</b></a>", helper.Action(action, controller), linkText);
        }

        public static string Link(this UrlHelper helper, string linkText, string action)
        {
            return string.Format("<a href=\"{0}\" class=\"sright\"><b class=\"by\">{1}</b></a>", helper.Action(action), linkText);
        }

        /// <summary>
        /// 条件为真是输出Html，否则返回空字符
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="condition"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string IsWrite(this HtmlHelper helper, bool condition, string html)
        {
            if (condition)
                return html;
            else
                return "";
        }

        public static string SwithWrite(this HtmlHelper helper, bool condition, string tureHtml, string falshHtml)
        {
            if (condition)
                return tureHtml;
            else
                return falshHtml;
        }

        public static void SwithWrite(this HtmlHelper helper, bool condition, Action tureHtml, Action falseHtml)
        {
            if (condition)
                tureHtml.Invoke();
            else
                falseHtml.Invoke();
        }
        //public static string Pager(this HtmlHelper helper, IPageOfList page)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendFormat(@"<ul class=""xianshi""><li>总计{0}条</li></ul><ul class=""navpage""><li>", page.RecordTotal);
        //    sb.Append(SimplePager(helper, page, "List", null, "上一页", "下一页"));
        //    sb.Append("</li></ul>");
        //    return sb.ToString();
        //}

        public static string Pager(this HtmlHelper helper, IPageOfList page)
        {
            if (page.RecordTotal == 0)
                return "<div>暂无相关记录</div>";

            var pageOfAList = page;
            StringBuilder sb = new StringBuilder("<div class=\"pager\">");
            ViewContext viewContext = helper.ViewContext;
            RouteValueDictionary rvd = new RouteValueDictionary();

            foreach (KeyValuePair<string, object> item in viewContext.RouteData.Values)
            {
                rvd.Add(item.Key, item.Value);
            }
            if (rvd.ContainsKey(SubmitName))
            {
                rvd.Remove(SubmitName);
                rvd.Remove("pageIndex");
            }

            var request = viewContext.HttpContext.Request;

            foreach (string key in request.QueryString)
            {
                rvd[key] = request.QueryString[key];
            }

            foreach (string key in request.Form)
            {
                rvd[key] = request.Form[key];
            }

            //如果当前不是第一页，则加上一页与第一页的标签
            if (page.PageIndex > 0)
            {
                sb.Append(GetLink("上一页", page.PageIndex - 1, rvd, helper));
                sb.Append(GetLink(0, rvd, helper));
            }

            //如果当前页是4以后的页，就加上省略号
            if (page.PageIndex >= 3)
                sb.Append("<b>...</b>");

            //如果当前是最后一页，并且总页数在两页以上，就加上倒数第二页的标签
            if (pageOfAList.PageIndex == pageOfAList.PageTotal - 1 && pageOfAList.PageTotal > 3)
                sb.Append(GetLink(pageOfAList.PageTotal - 3, rvd, helper));

            //如果当前页是第三页以后的，就加上他前一页的标签
            if (pageOfAList.PageIndex >= 2)
                sb.Append(GetLink(pageOfAList.PageIndex - 1, rvd, helper));

            //当前页
            sb.AppendFormat("<span>{0}</span>", pageOfAList.PageIndex + 1);

            //如果当前页小于倒数第二页，就加上后一页的标签
            if (pageOfAList.PageIndex < pageOfAList.PageTotal - 2)
                sb.Append(GetLink(page.PageIndex + 1, rvd, helper));

            //如果当前页是第一页，且总页数大于2就加上第3页的标签
            if (pageOfAList.PageIndex == 0 && pageOfAList.PageTotal > 3)
                sb.Append(GetLink(2, rvd, helper));

            //如果当前页是倒数第3页,就加上...
            if (pageOfAList.PageIndex < pageOfAList.PageTotal - 3)
                sb.Append("<b>...</b>");


            //如果当前页不是最后一页,就加下一页与最后页的标签
            if (pageOfAList.PageIndex < pageOfAList.PageTotal - 1)
            {
                sb.Append(GetLink(pageOfAList.PageTotal - 1, rvd, helper));
                sb.Append(GetLink("下一页", pageOfAList.PageIndex + 1, rvd, helper));
            }

            sb.Append("</div>");
            return sb.ToString();
        }

        public static string GetLink(string text, int pageIndex,RouteValueDictionary rvd, HtmlHelper helper)
        {
            rvd["pageIndex"] = pageIndex;
            return helper.ActionLink(text, rvd["action"].ToString(), rvd).ToString();
        }

        public static string GetLink(int pageIndex, RouteValueDictionary rvd, HtmlHelper helper)
        {
            return GetLink((pageIndex + 1).ToString(), pageIndex, rvd, helper);
        }

        //public static string DropDownList<T>(this HtmlHelper html, string name) where T : struct, IConvertible
        //{
        //    var list = Enum.GetNames(typeof(T)).Select(p => new SelectListItem { Text = ((Enum)(object)p.Parse<T>()).ToChinese(), Value = p.Parse<T>().ToString() }).ToList();
        //    return html.DropDownList(name, list);
        //}

        //public static string DropDownList<T>(this HtmlHelper html, string name, string label) where T : struct, IConvertible
        //{
        //    var list = Enum.GetNames(typeof(T)).Select(p => new SelectListItem { Text = ((Enum)(object)p.Parse<T>()).ToChinese(), Value = p.Parse<T>().ToString() }).ToList();
        //    return html.DropDownList(name, list, label);
        //}

        //public static string DropDownList<T>(this HtmlHelper html, string name,int index,SelectListItem item) where T : struct, IConvertible
        //{
        //    var list = Enum.GetNames(typeof(T)).Select(p => new SelectListItem { Text = ((Enum)(object)p.Parse<T>()).ToChinese(), Value = p.Parse<T>().ToString() }).ToList();
        //    list.Insert(index, item);
        //    return html.DropDownList(name, list);
        //}

        //public static string DropDownListInt<T>(this HtmlHelper html, string name) where T : struct, IConvertible
        //{
        //    var list = Enum.GetNames(typeof(T)).Select(p => new SelectListItem { Text = ((Enum)(object)p.Parse<T>()).ToChinese(), Value = Convert.ToInt32(p.Parse<T>()).ToString() }).ToList();
        //    return html.DropDownList(name, list);
        //}

    }

    
}
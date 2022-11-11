using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ProjectBase.Utils.Entities;
using System.Web.Routing;
using System.Web;
using System.Web.Mvc.Html;
using System.Globalization;

namespace ProjectBase.Utils.Web
{
    public static class PagingExtensions
    {
        public const string PageIndexName = "pageIndex";
        public static string Paging(this HtmlHelper helper, IPageOfList pageList)
        {
            if (pageList == null)
                throw new ArgumentNullException("pageList");

            if (pageList.PageTotal < 2)
                return "";

            RouteValueDictionary routeData = GetPagingRouteData(helper);
            string action = routeData["action"].ToString();

            StringBuilder result = new StringBuilder();
            if (pageList.PageIndex == 0)
                result.Append("<span class=\"prevDis\">上一页</span>");
            else
                result.Append(GetLink(helper, "上一页", action, "prevLink", GetPagingByPageNo(routeData, pageList.PageIndex - 1)));

            if (pageList.PageIndex == 0)
                result.Append(GetCurrentPageHtml(0));
            else
                result.Append(GetPageLink(helper,0,action,routeData));

            if (pageList.PageIndex == 1)
                result.Append(GetCurrentPageHtml(1));
            else
                result.Append(GetPageLink(helper, 1, action, routeData));

            if (pageList.PageIndex == 5)
                result.Append(GetPageLink(helper, 2, action, routeData));
            else if (pageList.PageIndex > 5)
                result.Append("<span class=\"more\">...</span>");

            for (int i = pageList.PageIndex - 2 > 2 ? pageList.PageIndex - 2 : 2; i < pageList.PageTotal && i <= pageList.PageIndex + 4; i++)
            {
                if (pageList.PageIndex == i)
                    result.Append(GetCurrentPageHtml(i));
                else
                    result.Append(GetPageLink(helper, i, action, routeData));
            }

            if (pageList.PageIndex + 4 < pageList.PageTotal - 3)
                result.Append("<span class=\"more\">...</span>");

            if (pageList.PageIndex + 4 < pageList.PageTotal - 2)
                result.Append(GetPageLink(helper, pageList.PageTotal - 2, action, routeData));

            if (pageList.PageIndex + 4 < pageList.PageTotal - 1)
                result.Append(GetPageLink(helper, pageList.PageTotal - 1, action, routeData));

            if (pageList.PageIndex == pageList.PageTotal - 1)
                result.Append("<span class=\"nextDis\">下一页</span>");
            else
                result.Append(GetLink(helper, "下一页", action, "nextLink", GetPagingByPageNo(routeData, pageList.PageIndex + 1)));

            return result.ToString();
        }

        private static RouteValueDictionary GetPagingByPageNo(RouteValueDictionary routeData, int pageIndex)
        {
            routeData[PageIndexName] = pageIndex;
            return routeData;
        }

        private static string GetCurrentPageHtml(int pageIndex)
        {
            return string.Format("<span class=\"current\">{0}</span>", pageIndex + 1);
        }

        private static string GetPageLink(this HtmlHelper helper, int pageIndex, string action, RouteValueDictionary routeData)
        {
            return GetLink(helper, (pageIndex + 1).ToString(), action, "pageNo", GetPagingByPageNo(routeData, pageIndex));
        }

        private static string GetLink(this HtmlHelper helper,string linktext, string action,string className, RouteValueDictionary routeData)
        {
            return helper.ActionLink(linktext, action, routeData).ToString().Replace("<a ", "<a class=\"" + className + "\" ");
        }

        public static RouteValueDictionary GetPagingRouteData(this HtmlHelper htmlHelper)
        {
            ViewContext viewContext = htmlHelper.ViewContext;
            RouteValueDictionary rvd = new RouteValueDictionary();
            HttpRequestBase request = viewContext.HttpContext.Request;


            foreach (string key in request.QueryString)
            {
                rvd[key] = request.QueryString[key];
            }

            foreach (string key in request.Form)
            {
                rvd[key] = request.Form[key];
            }

            foreach (KeyValuePair<string, object> item in viewContext.RouteData.Values)
            {
                rvd[item.Key] = item.Value;
            }

            rvd.Remove("message");
            rvd.Remove("pageIndex");
            rvd.Remove("search");
            return rvd;
        }

        public static string PageInfo(this HtmlHelper htmlHelper, IPageOfList pageList)
        {
            if (pageList.RecordTotal == 0)
                return "暂无相关记录";
            else
                return string.Format("总共有记录{0}条，共{1}页,每页显示{2}条,当前第{3}页", pageList.RecordTotal, pageList.PageTotal, pageList.PageSize, pageList.PageIndex + 1);
        }
    }
}

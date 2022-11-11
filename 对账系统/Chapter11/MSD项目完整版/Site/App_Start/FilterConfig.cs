using Site.Filters;
using System.Web;
using System.Web.Mvc;

namespace Site
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {       
            //默认的异常记录类
            //filters.Add(new HandleErrorAttribute());

            // ExceptionLogAttribute继承自HandleError，主要作用是将异常信息写入日志系统中
            filters.Add(new ExceptionLogAttribute());
        }
    }
}
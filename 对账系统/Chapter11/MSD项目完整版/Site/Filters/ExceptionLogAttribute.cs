using log4net;
using ProjectBase.Utils.Entities;
/* ==============================================================================
   * 功能描述：ExceptionLogAttribute  
   * 创 建 者：Zouqj
   * 创建日期：2015/5/28 18:16:05
   ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Site.Filters
{
    /// <summary>
    /// 异常持久化类
    /// </summary>
    public class ExceptionLogAttribute : HandleErrorAttribute
    {
        //public static Queue<string> MessageQueue = new Queue<string>();  
        private static readonly ILog logger = log4net.LogManager.GetLogger("WebLogger");
        private static readonly SysInfo _sysInfo = SysInfo.SysInfoModel;

        /// <summary>
        /// 触发异常时调用的方法
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (_sysInfo == null || _sysInfo.IsLogEnabled == "false")
            {

            }
            else
            {
                //<br>引发异常的方法：{2}<br>引发异常的对象：{3}<br>异常目录：{4}<br>异常文件：{5}
                string message = string.Format("消息类型：{0}\r\n消息内容：{1},\r\n引发异常的方法：{2}\r\n引发异常的对象：{3}\r\n异常控制器：{4}\r\n异常方法：{5} \r\n"
                    , filterContext.Exception.GetType().Name
                    , filterContext.Exception.Message
                    , filterContext.Exception.TargetSite
                    , filterContext.Exception.Source
                    , filterContext.RouteData.GetRequiredString("controller")
                    , filterContext.RouteData.GetRequiredString("action"));

                logger.Error(message); //将异常信息写入Log4Net中  
                //filterContext.HttpContext.Response.Redirect("~/Error.html");
            }


            base.OnException(filterContext);  
        }
    }
}

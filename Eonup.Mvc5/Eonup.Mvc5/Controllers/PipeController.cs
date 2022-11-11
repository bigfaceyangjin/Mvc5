using Eonup.Mvc5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Eonup.Mvc5.Controllers
{
	[AllowAnonymous]
    public class PipeController : Controller
    {
		//public delegate void TestDelegate(int x);
		//public event TestDelegate testEvent;
        // GET: Pipe
        public ActionResult Index()
        {
            return View();
        }
		#region HttpModule
		public ViewResult PipeModel()
		{
			//testEvent += x => base.Response.Write("<h1>testEvent</h1>");//订阅
			//testEvent(1);//触发

			//Application对象提供了25个事件 正常执行22个 个别什么Error等得被动触发
			//可以通过IHttpModule给任何一个事件扩展动作	要生效 别忘了WebConfig的配置文件 的system.webServer 的modules节点
			//HttpModule针对任何请求 只要是Application处理的，都可以扩展生效
			//用途：1.Session鉴权 2.性能监控(计算页面响应时间) 3.黑白名单() 4.请求日志 5.防爬虫 
			//理论上什么事可以做，但是不够精细，且损耗性能
			//可以处理部分系统module提升性能
			HttpApplication app = base.HttpContext.ApplicationInstance;
			List<SysEvent> eventList = new List<SysEvent>();
			int i = 0;
			foreach (EventInfo e in app.GetType().GetEvents())
			{
				i++;
				eventList.Add(new SysEvent() {
					id = i,
					Name=e.Name,
					TypeName=e.GetType().Name
				});
			}
			return View(eventList);
		}
		#endregion

		#region HttpHandler
		public ActionResult Handler()
		{
			ViewBag.handler= base.HttpContext.Handler.GetType().FullName;
			return View();
		}
		#endregion
	}
}
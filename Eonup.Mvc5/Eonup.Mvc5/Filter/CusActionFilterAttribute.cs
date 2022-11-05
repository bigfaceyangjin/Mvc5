using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eonup.Mvc5.Filter
{
	public class CusActionFilterAttribute:ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			filterContext.HttpContext.Response.Write($"<h2>这里是OnActionExecuting {DateTime.Now.ToString()}</h2>");
		}
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			filterContext.HttpContext.Response.Write($"<h2>这里是OnActionExecuted {DateTime.Now.ToString()}</h2>");
		}
		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			filterContext.HttpContext.Response.Write($"<h2>这里是OnResultExecuting {DateTime.Now.ToString()}</h2>");
		}
		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			filterContext.HttpContext.Response.Write($"<h2>这里是OnResultExecuted {DateTime.Now.ToString()}</h2>");
		}
	}
}
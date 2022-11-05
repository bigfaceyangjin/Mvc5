using Eonup.Framework.Log4;
using Eonup.Mvc5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eonup.Mvc5.Filter
{
	public class CusAuthorize:AuthorizeAttribute
	{
		public Logger logger = new Logger(typeof(CusAuthorize));
		/// <summary>
		/// 在Action方法执行前调用
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			var context = filterContext.HttpContext;
			//检查是否有匿名特性(避开登录权限的)
			if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)||filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute),true))
			{
				logger.Info($"{context.Request.Url} 匿名访问");
				return;
			}
			else if (context.Session["CurrentUser"] != null && context.Session["CurrentUser"] is CurrentUser)
			{
				logger.Info($"{((CurrentUser)context.Session["CurrentUser"]).Name} 访问 {context.Request.Url}");
				return;
			}
			else {
				logger.Info($"访问 {context.Request.Url} 未登录授权");
				filterContext.Result = new RedirectResult("/Six/Login");

				//访问某页面未登录授权时，保存此次访问的URL，以便登录成功后直接进入此URL
				context.Session.Remove("CurrentUrl");
				context.Session["CurrentUrl"] = context.Request.Url;
			}
		}
	}
}
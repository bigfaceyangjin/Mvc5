using Eonup.Framework.Log4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eonup.Mvc5.Filter
{
	public class CusHandleErrorAttribute: HandleErrorAttribute
	{
		private Logger logger = new Logger(typeof(CusHandleErrorAttribute));
		/// <summary>
		/// 异常后会进入的方法
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnException(ExceptionContext filterContext)
		{
			logger.Error($"访问{filterContext.HttpContext.Request.Url} 异常 原因:{filterContext.Exception.Message}");
			//判断是否处理过异常
			if (!filterContext.ExceptionHandled)
			{
				if (filterContext.HttpContext.Request.IsAjaxRequest())
				{
					filterContext.Result = new JsonResult()
					{
						Data = new { result = 0, msg = filterContext.Exception.Message }
					};
				}
				else
				{
					filterContext.Result = new ViewResult()
					{
						ViewName = "~/Views/Shared/Error.cshtml",
						ViewData = new ViewDataDictionary<string>(filterContext.Exception.Message)
					};
				}
				filterContext.ExceptionHandled = true;
			}
			//base.OnException(filterContext);
		}
	}
}
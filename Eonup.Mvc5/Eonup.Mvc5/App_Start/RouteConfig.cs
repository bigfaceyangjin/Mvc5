using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Eonup.Mvc5
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			//忽略路由
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			//routes.MapRoute(
			//	name: "Second",
			//	url: "main",
			//	defaults: new { controller = "Second", action = "Index", id = UrlParameter.Optional }
			//	);

			//routes.MapRoute(
			//	name: "Test",
			//	url: "Test/{action}/{id}",
			//	defaults: new { controller = "Second", action = "Index", id = UrlParameter.Optional }
			//	);

			//routes.MapRoute(
			//	name: "Regex",
			//	url: "{Controller}/{action}_{year}_{month}_{day}",
			//	defaults: new { controller = "Second", action = "Index", id = UrlParameter.Optional },
			//	constraints:new { year=@"\d{4}", month = @"\d{2}" ,day = @"\d{2}" }
			//	);
			//默认路由 
			routes.MapRoute(
				name: "Default",//路由名称
				url: "{controller}/{action}/{id}",//url的正则规则 去掉域名和端口后的地址 eg:Home/Index
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },//默认值
				namespaces:new string[] { "Eonup.Mvc5.Controllers" }
			);

			
		}
	}
}

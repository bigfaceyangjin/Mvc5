using Eonup.Framework.Log4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Eonup.Mvc5
{
	/// <summary>
	/// 1.MVC5：Controller、Action、View
	/// 2.Route、Area、Global
	/// 3.数据传值的多种方式
	/// 
	/// 核心文件夹：Models：存放实体 专门跟UI交互的
	///				View：UI层的，是html+后台代码组成 
	///				Controller:控制器，Action(对应cshtml)
	/// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
		private Logger logger = new Logger(typeof(MvcApplication));
		/// <summary>
		/// 网站第一次启动时执行一次，而且不再执行了
		/// dll更新/Webconfig修改 都会导致网站重启，此处重新执行
		/// </summary>
        protected void Application_Start()
        {
			logger.Info("网站第一次启动");
            AreaRegistration.RegisterAllAreas();//注册区域
            GlobalConfiguration.Configure(WebApiConfig.Register);//注册WebApi
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);//注册Filter
            RouteConfig.RegisterRoutes(RouteTable.Routes);//注册路由
            BundleConfig.RegisterBundles(BundleTable.Bundles);//捆绑css js
        }
    }
}

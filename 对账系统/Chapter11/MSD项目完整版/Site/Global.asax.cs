using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Reflection;
using System.Threading;
using System.IO;
using Site.Filters;
using log4net;
using ProjectBase.Utils.Entities;
using System.Web.Security;
using System.Security.Principal;

namespace Site
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleTable.EnableOptimizations = true;
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            RegisterContainer(ProjectBase.Data.IocContainer.Instance.Container);
            log4net.Config.XmlConfigurator.Configure();

            SysInfo.SysInfoModel = new SysInfo().GetSysConfigInfo();
        }

        public static void RegisterContainer(UnityContainer container)
        {
            var iRepositories = Assembly.Load("MSD.Finance.Core").GetTypes()
                .Where(p => p.FullName.Contains("Repositories")).ToList();
            var repositories = Assembly.Load("MSD.Finance.Data").GetTypes()
                .Where(p => p.FullName.Contains("Repository")).ToList();
            iRepositories.ForEach(
                p => container.RegisterType(p, repositories.Where(r => r.Name == p.Name.Substring(1, p.Name.Length - 1)).FirstOrDefault()));
        }
    }
}
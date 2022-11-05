using Eonup.Mvc5.Filter;
using System.Web;
using System.Web.Mvc;

namespace Eonup.Mvc5
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			//filters.Add(new CusAuthorize());  //全局注册登录权限
			filters.Add(new CusHandleErrorAttribute());
		}
	}
}

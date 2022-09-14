using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eonup.Mvc5.Controllers
{
    public class SecondController : Controller
    {
        // GET: Second
        public ActionResult Index()
        {
            return View();
        }
		public ActionResult Razor()
		{
			return View();
		}
		/// <summary>
		/// 扩展控件，就是用来生产html代码的，后台编写html，唯一的好处就是保证了标签的闭合，统一写法
		/// </summary>
		/// <returns></returns>
		public ActionResult HtmlExtend()
		{
			return View();
		}
	}
}
using Eonup.Mvc5.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eonup.Mvc5.Controllers
{
	[CusAuthorize]
	public class HomeController : Controller
	{
		public HomeController()
		{
			throw new Exception("Controller Exception");
		}
		public ActionResult Index()
		{
			ViewBag.Title = "Index Test";
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}
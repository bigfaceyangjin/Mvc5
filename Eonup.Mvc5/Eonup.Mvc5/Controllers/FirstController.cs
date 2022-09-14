using Eonup.Framework.Log4;
using Eonup.Mvc5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eonup.Mvc5.Controllers
{
	/// <summary>
	/// 数据的传值方式：两者都是字典，属性相同是会重叠的，后赋值的会覆盖 两者重定向后数据会丢失
	/// ViewData: ViewData.Add(new KeyValuePair(){"",""});ViewData["key"]=value;
	/// ViewBag: dynamic：动态类型 Framework4.0出现 弱类型（运行时确定，会避开编译器的检查） 强类型是已经确定的类型
	/// TempData:可以基于后台跨页面传值，是基于session的，但是只能使用一次，用了就没了
	/// </summary>
    public class FirstController : Controller
    {
		private Logger logger = new Logger(typeof(FirstController));

		//返回的实际上是一段字符串文本 包含response header 等内容
		//WebApi其实也一样 都是http协议 响应请求 返回数据
		//但是，WebApi是自宿主，管道模型不一样，HttpMethod支持不一样
        // GET: First
        public ActionResult Index()
        {
            return View();
        }
		public string GetStr(int? id,int s)
		{
			int k = id??2;
			return "加油";
		}
		public JsonResult GetJson()
		{
			return Json(new { result=true,msg="OK！"}, JsonRequestBehavior.AllowGet);
		}
		public ViewResult NoPara()
		{
			return View();
		}
		public ViewResult Para(int? id)
		{
			return View();
		}
		public ActionResult DataSmit(int? id)
		{
			logger.Info($"{nameof(FirstController)}_{nameof(DataSmit)}_{id}");
			ViewData.Add(new KeyValuePair<string, object>("k1", id ?? -1));
			ViewData["k2"] = "Just do it";
			ViewBag.CusUser = new CurrentUser() {
				Id = 7,
				Name = "IOC",
				Account = "限量版",
				Email = "莲花未开时",
				Password = "落单的候鸟",
				LoginTime = DateTime.Now
			};
			ViewBag.FName = "季雨林";
			TempData.Add("bigzhan", "zhuzhan");
			TempData["CusUser"] = new CurrentUser() {
				Id = 7,
				Name = "CSS",
				Account = "季雨林",
				Email = "KOKE",
				Password = "落单的候鸟",
				LoginTime = DateTime.Now
			};
			if (id == null)
				return base.Redirect("/First/Temp");
			else if (id == 1)
				return View();
			else if (id == 2)
				return View("~/Views/First/Data2.cshtml");
			else if (id == 3)
				return View("~/Views/First/Data3.cshtml",new CurrentUser() {
					Id = 7,
					Name = "CSS",
					Account = "季雨林",
					Email = "KOKE",
					Password = "落单的候鸟 ",
					LoginTime = DateTime.Now
				});
			return View();
		}
		public ActionResult Temp()
		{
			return View();
		}
		
    }
}
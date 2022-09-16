using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Eonup.EF.Model.Model;
using Eonup.Business.Interface;
using Eonup.Business.Service;

namespace Eonup.Mvc5.Controllers
{
	public class ThirdController : Controller
	{
		// GET: Third
		public ActionResult Index()
		{
			return View();
		}
		/// <summary>
		/// 分部视图
		/// 头尾结构重复：master→layout 布局页
		/// 不指定，默认就是:"~/Views/Shared/_layout.cshtml"
		/// </summary>
		/// <returns></returns>
		public ActionResult Partial()
		{
			return View();
		}
		public JsonResult Json1()
		{
			return new JsonResult() {
				Data = new {
					id = 1,
					name = "张三"
				},
				JsonRequestBehavior= JsonRequestBehavior.AllowGet
			};
		}
		public JsonResult Json2()
		{
			return Json(new {
				id = 1,
				name = "张三"
			},JsonRequestBehavior.AllowGet);
		}
		public void Json3()
		{
			HttpContext.Response.ContentType = "application/json";
			HttpContext.Response.Write(JsonConvert.SerializeObject(new {
				id = 1,
				name = "张三"
			}));
		}
		public XmlResult Xml1()
		{
			return new XmlResult(new {
				Id=1,
				Name="朱展"
			});
		}

		#region EF + DbContext、抽象、Unity容器、自动注入
		public ActionResult EFDb1()
		{
			using (NorthwindContext context = new NorthwindContext())
			{
				Customer customer = context.Customers.FirstOrDefault(c => c.CustomerID == "ALFKI");
				return View(customer);
			}

		}
		public ActionResult EFDb2()
		{
			using (ICustomersService service = new CustomersService(new NorthwindContext()))
			{
				Customer cus1 = service.Find<Customer>("ANTON");
				List<Customer> customerS = service.Set<Customer>().ToList<Customer>();
				return View(customerS);
			}
		}
		//public ActionResult EFDb3()
		//{

		//}
		#endregion

	}
	public class XmlResult : ActionResult
	{
		public XmlResult() { }
		private object _data = null;
		public XmlResult(object data)
		{
			this._data = data;
		}
		public override void ExecuteResult(ControllerContext context)
		{
			context.HttpContext.Response.ContentType = "text/xml";//application/xml
			new XmlSerializer(this._data.GetType()).Serialize(context.HttpContext.Response.OutputStream, this._data);
		}
	}
}
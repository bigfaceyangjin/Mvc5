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
using Eonup.Framework.Unity;
using Unity;
using Eonup.EF.Model;
using System.Web.Routing;
using Eonup.Mvc5.Models;

namespace Eonup.Mvc5.Controllers
{
	public class ThirdController : Controller
	{
		#region Identity
		//private ICustomersService _customersService = null;
		//public ThirdController(ICustomersService customersService)
		//{
		//	this._customersService = customersService;
		//}
		#endregion
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
			return new XmlResult(new XmlUser(){
				id=1,
				name="朱展"
			});
		}

		#region EF + DbContext、抽象、Unity容器、自动注入
		public ActionResult EFDb1()
		{
			//using (NorthwindContext context = new NorthwindContext())
			//{
			//	Customer customer = context.Customers.FirstOrDefault(c => c.CustomerID == "ALFKI");
			//	return View(customer);
			//}
			using (ZhanProDbContext context = new ZhanProDbContext())
			{
				Company company= context.Company.FirstOrDefault(c=>c.Id==6);
				return View(company);
			}

		}
		//public ActionResult EFDb2()
		//{
		//	//using (ICustomersService service = new CustomersService(new NorthwindContext()))
		//	//{
		//	//	Customer cus1 = service.Find<Customer>("ANTON");
		//	//	List<Customer> customerS = service.Set<Customer>().ToList<Customer>();
		//	//	return View(customerS);
		//	//}
		//	using (ICompanyService service = new CompanyService(new ZhanProDbContext()))
		//	{
		//		List<Bus_Bank> list= service.Set<Bus_Bank>().ToList();
		//		return View(list);
		//	}
		//}
		//public ActionResult EFDb3()
		//{
		//	using (ICompanyService service = ContainerFactory.CreateContainer().Resolve<ICompanyService>())
		//	{
		//		List<Company> list= service.Set<Company>().ToList();
		//		return View("EFDb2", list);
		//	}
		//}
		public ActionResult EFDb4()
		{
			try
			{
				Company company = null;// this._companyService.Find<Company>(1);
				return View(company);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		#endregion

	}
	public class BigZhanControllerFactory : DefaultControllerFactory
	{
		protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
		{
			return (IController)ContainerFactory.CreateContainer().Resolve(controllerType);
		}
	}
	/// <summary>
	/// 返回XML格式数据给前端
	/// </summary>
	public class XmlResult : ActionResult
	{
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
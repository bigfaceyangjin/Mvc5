using Eonup.Business.Interface;
using Eonup.Mvc5.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eonup.Mvc5.Controllers
{
	/// <summary>
	/// 测试以下能不能进入Filter捕获异常： T 能捕获到  F不能捕获到
	/// Exception			T
	/// Action内try catch	F
	/// ServiceException	T
	/// CotrollerException	F
	/// ViewException		T
	/// URL错误Exception	F
	/// 
	/// 其他异常怎么处理？	在global中使用Application_Error方法
	/// </summary>
    public class SevenController : Controller
    {
		#region Identity
		private ICommodityService _CommodityService = null;
		public SevenController(ICommodityService commodityService)
		{
			this._CommodityService = commodityService;
		}
		#endregion
		// GET: Seven
		//[CusHandleErrorAttribute]
		//[ActionFilter]
		public ActionResult Index()
        {
			int a = 0, b = 32;
			int c = b / a;
			throw new Exception("");
			return View();
        }
		public ActionResult Exception()
		{
			try
			{
				throw new Exception("这里是 Action Excetion");
				return View();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public ActionResult ServiceException()
		{
			this._CommodityService.Show();
			return View();
		}
		public ActionResult ViewException()
		{
			return View();
		}
		[CusActionFilterAttribute]
		public ActionResult ActionTest()
		{
			return View();
		}
	}
}
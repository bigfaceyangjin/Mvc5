using Eonup.Business.Interface;
using Eonup.EF.Model;
using Eonup.EF.Model.Model;
using Eonup.Framework;
using Eonup.Framework.ExtensionMethod;
using Eonup.Framework.Log4;
using Eonup.Remote.Interface;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Eonup.Mvc5.Controllers
{
    public class FourController : Controller
    {

		#region Identity
		Logger loger = new Logger(typeof(FourController));
		private ISearchService _searchService=null;
		private IProductService _productService = null;
		private ICommodityService _commodityService = null;
		public FourController(ISearchService searchService, IProductService productService, ICommodityService commodityService)
		{
			this._searchService = searchService;
			this._productService = productService;
			this._commodityService = commodityService;
		}
		#endregion
		private int pageSize=5;
		// GET: Four
		/// <summary>
		/// 商品列表
		/// </summary>
		/// <param name="search">搜索条件</param>
		/// <param name="pageIndex">分页索引</param>
		/// <returns></returns>
		public ActionResult Index(string search,int orderby =-1, int pageIndex = 1,bool Asc=true)
		{
			{
				//ViewBag.Data = this._searchService.Query(new int[] { 1}.ToList());
				//List<Product> list = this._productService.Set<Product>().ToList();
				//int count = list.Count;
				//if (!string.IsNullOrEmpty(search))
				//{
				//	list = list.Where(p => p.ProductName.Contains(search)).ToList();
				//	count = list.Count();
				//}
				//list =list.OrderBy(o => o.ProductID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
			}
			ViewBag.SearchString = search;
			Expression<Func<Commodity, bool>> lambdaWhere = null;
			if (!string.IsNullOrEmpty(search))
			{
				lambdaWhere= lambdaWhere.And(p => p.Title.Contains(search));
			}
			Expression<Func<Commodity, decimal>> lambdaOrderby = null;
			if (orderby == -1)
			{
				ViewBag.orderby = 1;
				lambdaOrderby = p => p.Id;//默认ID升序排序
			}
			else if (orderby == 0)
			{
				lambdaOrderby = p => p.Price;
				ViewBag.orderby = 1;
			}
			else {
				lambdaOrderby = p => p.Price;
				ViewBag.orderby = 0;
				Asc = false;
			}
			PageResult<Commodity> result= this._commodityService.Query<Commodity>(lambdaWhere, lambdaOrderby, pageIndex, pageSize,Asc);
			StaticPagedList<Commodity> pages = new StaticPagedList<Commodity>(result.dataList, pageIndex, pageSize, result.totalCount);

			return View(pages);
        }

		[HttpGet]
		public ActionResult Create()
		{
			var list= this.BuilderDropList();
			ViewBag.categoryList = this.BuilderDropList().ToList();
			//ViewBag.CategoryList = this.BuilderDropList<Category>(c=> new SelectListItem() {
			//	Text=c.CategoryName,
			//	Value=c.CategoryID.ToString()
			//});
			return View();
		}

		/// <summary>
		/// [Bind()]特性保证了不管你提交多少值,我只接收定义的值，保证了安全性
		///  ModelState.AddModelError()函数可以给Model添加一条错误信息，函数的第一个参数是key，用于查找这   个   错误信息，第二个参数是错误信息的具体内容。这个错误信息可以在View中通过						Html.ValidationMessage("unableToSave")来访问到。
		///页面上的Html.AntiForgeryToken()会给访问者一个默认名为__RequestVerificationToken的cookie
		/// 为了验证一个来自form post，还需要在目标action上增加[ValidateAntiForgeryToken]特性，它是一个验证过滤器，
		/// 它主要检查
		/// 
		/// (1)请求的是否包含一个约定的AntiForgery名的cookie
		/// 
		/// (2)请求是否有一个Request.Form["约定的AntiForgery名"]，约定的AntiForgery名的cookie和Request.Form值是否匹配
		/// </summary>
		/// <param name="product"></param>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "CategoryId,Title,Price,Url,ImageUrl")]Commodity commodity)
		{
			//ModelState不通过原因(调试使用)：
			if (!ModelState.IsValid)
			{
				List<string> list = new List<string>();
				List<string> keys= ModelState.Keys.ToList();
				foreach (var key in keys)
				{
					var errors= ModelState[key].Errors.ToList();
					foreach (var err in errors)
					{
						list.Add(err.ErrorMessage);
					}
				}
			}
			if (ModelState.IsValid)
			{
				Commodity _commodity= this._commodityService.Insert<Commodity>(commodity);
				return RedirectToAction("Index");
			}
			else {
				ModelState.AddModelError("ProError", "ModelState检测未通过");
				throw new Exception("Product ModelState检测未通过");
			}
		}

		[HttpPost]
		public JsonResult AjaxData(Commodity commodity)
		{
			Commodity com = this._productService.Insert(commodity);
			return Json(new { result=true,msg="新增成功！"});
		}
		/// <summary>
		/// 下拉框数据集合
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		public IEnumerable<SelectListItem> BuilderDropList()
		{
			var categoryList= this._commodityService.GetChildList<Category>(c => c.ParentCode.StartsWith("root"));
			if (categoryList.Count() > 0)
			{
				return categoryList.Select(x=>new SelectListItem() {
					Text=x.Name,
					Value=x.Id.ToString()
				});
			}
			return null;
		}
	}
}
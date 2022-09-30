using Eonup.Business.Service;
using Eonup.EF.Model;
using Eonup.Framework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Eonup.Mvc5.Controllers
{
    public class FifthController : Controller
    {
		#region Identity
		private CommodityService _commodityService = null;
		public FifthController(CommodityService commodityService)
		{
			this._commodityService = commodityService;
		}
		#endregion
		// GET: Fifth
		public ActionResult Index()
        {
			ViewBag.FirstList = BindCategoryList(this._commodityService.GetChildList<Category>(c => c.ParentCode.Equals("root")));
			ViewBag.SecondList = BindCategoryList(null);
			ViewBag.ThirdList = BindCategoryList(null);

			return View();
        }

		public ActionResult SearchPartialList(string searchString,int orderby =-1,int pageIndex=1,int firstDrop=-1, int secondDrop = -1, int thirdDrop = -1,bool asc=true)
		{
			//从第三级下拉框查，如果某级下拉框有选择，那就返回这级的CategoryId
			int dropId = thirdDrop == -1 ? (secondDrop==-1?(firstDrop==-1?-1:firstDrop):secondDrop) : thirdDrop;
			if (dropId == -1 && string.IsNullOrEmpty(searchString))
			{
				searchString = "男装";
			}
			Expression<Func<Commodity, decimal>> orderbyLambda = null;
			if (orderby == -1)
			{
				orderbyLambda = c => c.Id;
			}
			else if (orderby == 0)
			{
				orderbyLambda = c => c.Price;
				ViewBag.orderby = 1;
			}
			else if(orderby == 1)
			{
				orderbyLambda = c => c.Price;
				asc = false;
				ViewBag.orderby = 0;
			}
			//如果下拉框有选择，则必须查出该类别以及它所有的子类别,除了是thirdDrop不为-1，则不用查子级
			List<int> categoryIdList = new List<int> ();
			if (dropId != -1)
			{
				Category category = this._commodityService.CacheAllCategory<Category>().FirstOrDefault(c => c.Id == dropId);
				if (thirdDrop == -1)
				{
					//子级类别Id和本级Id(所有类别Id)
					categoryIdList= this._commodityService.GetChildList<Category>(c => c.ParentCode.StartsWith(category.Code)||c.Id==dropId).Select(c=>c.Id).ToList();
				}
				else {
					categoryIdList.Add(dropId);
				}
			}
			Expression<Func<Commodity, bool>> whereLambda = null;
			if (categoryIdList.Count > 0)
			{
				whereLambda = c => c.Title.Contains(searchString) && categoryIdList.Contains(c.CategoryId);
			}
			else {
				whereLambda = c => c.Title.Contains(searchString);
			}
			
			PageResult<Commodity> result= this._commodityService.Query<Commodity>(whereLambda,orderbyLambda, pageIndex, 5);
			StaticPagedList<Commodity> page = new StaticPagedList<Commodity>(result.dataList, pageIndex, 5, result.totalCount);
			return View(page);
		}
		/// <summary>
		/// 查询二级菜单
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public JsonResult CategoryDropBind(int id)
		{
			//根据id查出code
			Category category= this._commodityService.Find<Category>(id);
			string pCode= category.Code;
			//将id的code作为parentcode查出下一级的所有category
			IEnumerable<SelectListItem> categories= this._commodityService.Set<Category>().Where(c => c.ParentCode.Equals(pCode)).Select(s=>new SelectListItem() {
				Text=s.Name,
				Value=s.Id.ToString()
			});
			return Json(new { result=true,data= categories });
		}
		

		#region Private Method
		/// <summary>
		/// 二级菜单列表绑定
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		private IEnumerable<SelectListItem> BindCategoryList(IEnumerable<Category> list)
		{
			List<SelectListItem> listItems = new List<SelectListItem>() {
				new SelectListItem()
					{
						Text = "--请选择--",
						Value = "-1",
						Selected = true
					}
			};
			if (list != null && list.Count() > 0)
			{
				listItems.AddRange(list.Select(s => new SelectListItem()
				{
					Text = s.Name,
					Value = s.Id.ToString(),
					Selected = false
				}));
			}
			return listItems;
		}
		#endregion

		#region Query
		/// <summary>
		/// 商品详情页
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult Details(int? Id)
		{
			if (Id == null)
			{
				throw new Exception("Need Commodity Id");
			}
			Commodity t = this._commodityService.Find<Commodity>(Id ?? -1);
			if (t == null)
			{
				throw new Exception("Not Defined Commodity");
			}
			return View(t);
		}
		[HttpGet]
		public ActionResult Edit(int Id)
		{
			Commodity commodity = this._commodityService.Find<Commodity>(Id);
			ViewBag.CategoryList = this._commodityService.GetChildList<Category>(c => c.ParentCode.StartsWith("root"));
			return View(commodity);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,CategoryId,ProductId,Title,Price,Url,ImageUrl")]Commodity commodity)
		{
			if (ModelState.IsValid)
			{
				Commodity comm = this._commodityService.Find<Commodity>(commodity.Id);
				comm.ProductId = commodity.ProductId;
				comm.CategoryId = commodity.CategoryId;
				comm.Title = commodity.Title;
				comm.Price = commodity.Price;
				comm.Url = commodity.Url;
				comm.ImageUrl = commodity.ImageUrl;
				this._commodityService.Update<Commodity>(comm);
				return RedirectToAction("Index", "Four");
			}
			else
			{
				throw new Exception("ModelState 验证未通过！");
			}
		}

		public ActionResult Delete(int Id)
		{
			Commodity comm = this._commodityService.Find<Commodity>(Id);
			if (comm == null)
			{
				throw new Exception("Entity is null");
			}
			else
			{
				this._commodityService.Delete<Commodity>(comm);
				return RedirectToAction("Index", "Four");
			}
		}
		[HttpPost]
		public JsonResult AjaxDelete(int Id)
		{
			Commodity comm = this._commodityService.Find<Commodity>(Id);
			this._commodityService.Delete<Commodity>(comm);
			return Json(new
			{
				result = true,
				msg = "删除成功"
			});
		}
		#endregion
	}
}
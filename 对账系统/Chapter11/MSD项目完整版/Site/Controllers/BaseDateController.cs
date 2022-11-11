using Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    [Authorize]
    public class BaseDateController : Controller
    {
        /// <summary>
        /// 币种Q
        /// </summary>
        /// <returns></returns>
        public ActionResult Currency()
        {
            return View();
        }
        /// <summary>
        /// 币种列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CurrencyList(CurrencyFilter filter)
        {
            var dataSource = Core.BaseData.Currency.GetByFilter(filter);
            int maxRows = (int)dataSource.RecordTotal;

            filter.PageSize = maxRows;

            var queryData = dataSource.ToList();

            var data = queryData.Select(u => new
            {
                ID = u.ID,
                FullName = u.FullName,
                ShortName = u.ShortName,
                ExchangeRate = u.ExchangeRate,
                Symbol = u.Symbol,
                CreateUser = u.CreateUser,
                CreateTime = u.CreateTime,
                VoidFlag = u.VoidFlag,
                IsDefault = u.IsDefault,
            });
            //构造成Json的格式传递
            var result = new { iTotalRecords = queryData.Count, iTotalDisplayRecords = 10, data = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加币种
        /// </summary>
        /// <returns></returns>
        public ActionResult CurrencyAdd()
        {
            return View();
        }

        /// <summary>
        /// 收费项
        /// </summary>
        /// <returns></returns>
        public ActionResult FeeItem()
        {
            return View();
        }

    }
}

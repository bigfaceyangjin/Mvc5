using Core.CostFlow;
using Core.Filters;
using ProjectBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    [Authorize]
    public class IdentityCardCostController : Controller
    {
        /// <summary>
        /// 身份证费用
        /// </summary>
        /// <returns></returns>
        public ActionResult IdentityCardCost()
        {
            return View();
        }
        /// <summary>
        /// 身份证费用列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IdentityCardCostList(IdentityCardDetailFilter filter)
        {
            filter.OrderBy = "ID desc";

            //filter.Source = (int)OrderSource.APIFastGood; //过滤 只加载走快件的单
            long totalRowCounts = Core.CostFlow.IdentityCardDetail.GetByFilter(filter).RecordTotal; //总行数
            var displayTotalRows = Core.CostFlow.IdentityCardDetail.GetByFilter(filter).Count; //页行数

            DataTablesRequest parm = new DataTablesRequest(this.Request);    //处理对象
            int pageIndex = parm.iDisplayStart / parm.iDisplayLength;
            filter.PageIndex = pageIndex;    //页索引
            filter.PageSize = parm.iDisplayLength;    //页行数
            var DataSource = Core.CostFlow.IdentityCardDetail.GetByFilter(filter);
            List<IdentityCardDetail> queryData = DataSource.ToList();

            //var dataSource = Core.CostFlow.IdentityCardDetail.GetByFilter(filter);
            //int maxRows = (int)dataSource.RecordTotal;
            //filter.PageSize = maxRows;
            //var queryData = dataSource.ToList();
            var data = queryData.Select(u => new
            {
                ID = u.ID,
                Cus_Name = u.CustomerInfoBy.CusName,
                IdentityName = u.IdentityName,
                IdentityNmber = u.IdentityNmber,
                ValideTime = u.ValideTime.ToStringDate(),
                Fee = u.Fee,
                Status = u.Status.GetDescription(false),              
            });
            //构造成Json的格式传递
            var result = new { iTotalRecords = queryData.Count, iTotalDisplayRecords = 10, data = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

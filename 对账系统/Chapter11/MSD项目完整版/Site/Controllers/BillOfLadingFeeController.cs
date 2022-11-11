using Core.Filters;
using Core.Receivable;
using ProjectBase.Utils;
using ProjectBase.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    [Authorize]
    public class BillOfLadingFeeController : Controller
    {
        //
        // GET: /BillOfLadingFee/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(BillOfLadingFeeFilter filter)
        {
            filter.OrderBy = "ID desc";

            //filter.Source = (int)OrderSource.APIFastGood; //过滤 只加载走快件的单
            long totalRowCounts = LoadBillInCome.GetByFilter(filter).RecordTotal; //总行数
            var displayTotalRows = LoadBillInCome.GetByFilter(filter).Count; //页行数

            DataTablesRequest parm = new DataTablesRequest(this.Request);    //处理对象

            int pageIndex = parm.iDisplayStart / parm.iDisplayLength;

            filter.PageIndex = pageIndex;    //页索引
            filter.PageSize = parm.iDisplayLength;    //页行数

            var DataSource = LoadBillInCome.GetByFilter(filter);
            List<LoadBillInCome> queryData = DataSource.ToList();

            var data = queryData.Select(u => new
            {
                ID = u.ID,
                CusName = u.CustomerInfoBy == null ? "" : u.CustomerInfoBy.CusName,
                LoadBillNum = u.LoadBillNum,
                BillStatus = u.BillStatus.ToChinese(),
                CompletionTime = u.CompletionTime.ToStringDate(),
                OrderCounts = u.OrderCounts,
                BillWeight = u.BillWeight,
                LoadFee = u.LoadFee,
                StoreFee = u.StoreFee,
                TotalCollectFees = u.TotalCollectFees,
                TotalOperateFee = u.TotalOperateFee,
                OtherFee = u.OtherFee,
                PayStatus = u.PayStatus.ToChinese(),
                TotalFee = u.LoadFee + u.StoreFee + u.TotalCollectFees + u.TotalOperateFee + u.OtherFee
            });
            //构造成Json的格式传递
            var result = new { iTotalRecords = displayTotalRows, iTotalDisplayRecords = totalRowCounts, data = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

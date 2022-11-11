using Core.Filters;
using Core.Receivable;
using ProjectBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    [Authorize]
    public class ReceivableController : Controller
    {
        public ActionResult TaxSettlement()
        {
            return View();
        }

        [HttpPost]
        public JsonResult TaxSettlementList(TaxDetailFilter filter)
        {
            DataTablesRequest parm = new DataTablesRequest(this.Request);    //处理对象
            int pageIndex = parm.iDisplayStart / parm.iDisplayLength;
            filter.PageIndex = pageIndex;    //页索引
            filter.PageSize = parm.iDisplayLength;    //页行数
            var DataSource = TaxDetail.GetByFilter(filter);

            int i = DataSource.PageSize * DataSource.PageIndex;
            List<TaxDetail> queryData = DataSource.ToList();

            var data = queryData.Select(u => new
            {
                ID = u.ID, 
                Index = ++i, //行号
                CusName = u.CustomerInfoBy == null ? "" : u.CustomerInfoBy.CusName,//客户简称
                LoadBillNum = u.LoadBillNum,//提单号
                ExpressNo = u.ExpressNo,//运单号
                TaxBillNO = u.TaxBillNO,//税单号
                TaxFee = u.TaxFee,//税金
                AffirmTime = u.AffirmTime.ToStringDate(),//税费确认时间
                InputTime = u.InputTime.ToStringDate(),//税费导入时间
                PayStatus = u.PayStatus.GetDescription(false)//结算状态
            });
            //构造成Json的格式传递
            var result = new { iTotalRecords = DataSource.Count, iTotalDisplayRecords = DataSource.RecordTotal, data = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

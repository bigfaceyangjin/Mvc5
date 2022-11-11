using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSD.WL.Site.Models;

namespace MSD.WL.Site.Controllers
{
    public class ChannelController : Controller
    {
        //
        // GET: /Channel/

        public ActionResult Index()
        {
            return View();
        }
        //添加渠道
        public ActionResult AddChannel()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(ChannelInfo filter)
        {
            List<ChannelInfo> list = new List<ChannelInfo>();
            for (int i = 0; i < 1100; i++)
            {
                list.Add(new ChannelInfo
            {
                ID = 1,
                ChannelCode = "E_Express"+i,
                ChannelStyle = "香港E特快"+i,
                CnName = "香港E特快"+i,
                EnName = "HK E-Express"+i,
                Status = "1"
            });
            }
            if (!string.IsNullOrEmpty(filter.ChannelCode))
            {
                list = list.Where(x => x.ChannelCode == filter.ChannelCode.Trim()).ToList();
            }
            if (!string.IsNullOrEmpty(filter.CnName))
            {
                list = list.Where(x => x.CnName == filter.CnName.Trim()).ToList();
            }
            if (!string.IsNullOrEmpty(filter.EnName))
            {
                list = list.Where(x => x.EnName == filter.EnName.Trim()).ToList();
            }

            //构造成Json的格式传递
            var result = new { iTotalRecords = 1100, iTotalDisplayRecords = 10, data = list };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

using Core.CostFlow;
using Core.Filters;
using Core.Reconciliation;
using Data.Reconciliation;
using ProjectBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectBase.Utils.Entities;
using Core;
using Core.Receivable;
using Site.Models;
using Core.ExceptionDir;
using System.Transactions;

namespace Site.Controllers
{
    [Authorize]
    public class ReconciliationController : Controller
    {
        private string message = "<script>frameElement.api.opener.hidePublishWin('{0}', '{1}','{2}'); </script>"; //消息，是否关闭弹出窗，是否停留在当前分页（0，1）
        Dictionary<int, string> dicSort = new Dictionary<int, string>(); //排序字段键值对列表 （列序号，列名称）

        #region 运单对账
        //运单对账
        public ActionResult WayBill(WayBillReconciliationFilter filter)
        {
            //ViewBag.ExceptionCount = Core.Reconciliation.WayBillException.GetExceptionCount(filter);

            return View();
        }

        [HttpPost]
        public JsonResult WayBillList(WayBillReconciliationFilter filter)
        {
            WayBillExceptionFilter exceptionfilter = new WayBillExceptionFilter() { CusShortName = filter.CusShortName, LoadBillNum = filter.LoadBillNum, ExpressNo = filter.ExpressNo, PostingTime = filter.PostingTime, PostingTimeTo = filter.PostingTimeTo };
            long counts = Core.Reconciliation.WayBillException.GetExceptionCount(exceptionfilter);
            //
            DataTablesRequest parm = new DataTablesRequest(this.Request);    //处理对象
            int pageIndex = parm.iDisplayLength == 0 ? 0 : parm.iDisplayStart / parm.iDisplayLength;
            filter.PageIndex = pageIndex;    //页索引
            filter.PageSize = parm.iDisplayLength;    //页行数
            var DataSource = WayBillReconciliation.GetByFilter(filter) as WRPageOfList<WayBillReconciliation>;

            int i = parm.iDisplayLength * pageIndex;

            List<WayBillReconciliation> queryData = DataSource.ToList();
            var data = queryData.Select(u => new
            {
                Index = ++i, //行号
                ID = u.ID,
                IsInputCost = u.IsInputCost,
                CusName = u.CusName, //客户简称
                PostingTime = u.PostingTime == null ? string.Empty : u.PostingTime.Value.ToStringDate(),//收寄日期
                ExpressNo = u.ExpressNo, //运单号
                BatchNO = u.LoadBillNum, //提单号
                Weight = u.Weight == null ? 0m : u.Weight / 1000, //重量
                WayBillFee = u.WayBillFee, //邮资
                ProcessingFee = u.ProcessingFee, //邮政邮件处理费
                InComeWayBillFee = u.ExpressFee, //客户运费
                InComeOprateFee = u.OperateFee, //客户操作费
                WayBillMargins = u.WayBillProfit, //运费毛利
                TotalMargins = u.ExpressFee + u.OperateFee + u.InComeOtherFee - (u.WayBillFee + u.ProcessingFee + u.CostOtherFee), //总毛利
                Margin = Math.Round((u.ExpressFee + u.OperateFee + u.InComeOtherFee == 0 ? 0m : (u.ExpressFee + u.OperateFee + u.InComeOtherFee - (u.WayBillFee + u.ProcessingFee + u.CostOtherFee)) / (u.ExpressFee + u.OperateFee + u.InComeOtherFee) * 100), 2), //毛利率 毛利率=(总收入-总的支出的成本)/总收入*100% 
                ReconcileDate = u.ReconcileDate.ToStringDate(), //对账日期
                CostOtherFee = u.CostOtherFee, //成本 其他费用
                CostTotalFee = u.WayBillFee + u.ProcessingFee + u.CostOtherFee, //成本 总费用
                CostStatus = u.CostStatus.ToChinese(),  //成本 状态
                InComeOtherFee = u.InComeOtherFee, //收入 其他费用
                InComeTotalFee = u.ExpressFee + u.OperateFee + u.InComeOtherFee, //收入 总费用
                InComeStatus = u.InComeStatus.ToChinese(),  //收入 状态
                Statement = u.Statement  //对账单状态
            });
            //构造成Json的格式传递
            var result = new
            {
                ExceptionCount = counts,
                iTotalRecords = DataSource.Count,
                iTotalDisplayRecords = DataSource.RecordTotal,
                data = data,
                TotalWeight = DataSource.StatModelBy.TotalWeight / 1000,
                TotalWayBillFee = DataSource.StatModelBy.TotalWayBillFee,
                TotalProcessingFee = DataSource.StatModelBy.TotalProcessingFee,
                TotalExpressFee = DataSource.StatModelBy.TotalExpressFee,
                TotalOperateFee = DataSource.StatModelBy.TotalOperateFee,
                SumWayBillProfit = DataSource.StatModelBy.TotalWayBillProfit,
                SumTotalProfit = 0m //总毛利求和
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 运单异常数据
        /// </summary>
        /// <returns></returns>
        public ActionResult WayBillException(WayBillExceptionFilter filter)
        {
            return View(filter);
        }

        public JsonResult WayBillExceptionList(WayBillExceptionFilter filter)
        {
            dicSort.Add(2, "w.PostingTime");

            DataTablesRequest parm = new DataTablesRequest(this.Request);    //处理对象
            int pageIndex = parm.iDisplayLength == 0 ? 0 : parm.iDisplayStart / parm.iDisplayLength;
            filter.PageIndex = pageIndex;    //页索引
            filter.PageSize = parm.iDisplayLength;    //页行数

            string strSortField = dicSort.Where(x => x.Key == parm.SortColumns[0].Index).Select(x => x.Value).FirstOrDefault();
            string strSortDire = parm.SortColumns[0].Direction == SortDirection.Asc ? "asc" : "desc";

            filter.OrderBy = " " + strSortField + " " + strSortDire;

            var DataSource = Core.Reconciliation.WayBillException.GetByFilter(filter) as WRPageOfList<WayBillException>;

            int i = parm.iDisplayLength * pageIndex;

            List<WayBillException> queryData = DataSource.ToList();
            var data = queryData.Select(u => new
            {
                Index = ++i, //行号
                ID = u.ID,
                IsInputCost = u.IsInputCost,
                CusName = u.CusName, //客户简称
                PostingTime = u.PostingTime == null ? string.Empty : u.PostingTime.Value.ToStringDate(),//收寄日期
                ExpressNo = u.ExpressNo, //运单号
                BatchNO = u.LoadBillNum, //提单号
                Weight = u.Weight == null ? 0m : u.Weight / 1000, //重量
                WayBillFee = u.WayBillFee, //邮资
                ProcessingFee = u.ProcessingFee, //邮政邮件处理费
                InComeWayBillFee = u.ExpressFee, //客户运费
                InComeOprateFee = u.OperateFee, //客户操作费
                WayBillMargins = u.WayBillProfit, //运费毛利
                TotalMargins = u.ExpressFee + u.OperateFee + u.InComeOtherFee - (u.WayBillFee + u.ProcessingFee + u.CostOtherFee), //总毛利
                Margin = Math.Round((u.ExpressFee + u.OperateFee + u.InComeOtherFee == 0 ? 0m : (u.ExpressFee + u.OperateFee + u.InComeOtherFee - (u.WayBillFee + u.ProcessingFee + u.CostOtherFee)) / (u.ExpressFee + u.OperateFee + u.InComeOtherFee) * 100), 2) + "%", //毛利率 毛利率=(总收入-总的支出的成本)/总收入*100% 
                ReconcileDate = u.ReconcileDate.ToStringDate(), //对账日期
                CostOtherFee = u.CostOtherFee, //成本 其他费用
                CostTotalFee = u.WayBillFee + u.ProcessingFee + u.CostOtherFee, //成本 总费用
                CostStatus = u.CostStatus.ToChinese(),  //成本 状态
                InComeOtherFee = u.InComeOtherFee, //收入 其他费用
                InComeTotalFee = u.ExpressFee + u.OperateFee + u.InComeOtherFee, //收入 总费用
                InComeStatus = u.InComeStatus.ToChinese(),  //收入 状态
                ExceptionMsg = u.ExceptionMsg, //运单异常原因
                WayBillCostID = u.WayBillCostID //运单成本ID
                // ExceptionType = u.ExceptionType  //运单异常状态
            });
            //decimal totalProfit = 0m;      //总毛利求和
            //构造成Json的格式传递
            var result = new
            {
                iTotalRecords = DataSource.Count,
                iTotalDisplayRecords = DataSource.RecordTotal,
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult WayBillExceptionDelete(int wayBillCostID, int ID)
        {

            //ViewBag.Title = "删除异常数据";
            //ViewBag.ExpressNo = wayBillCostID;

            //异常数据对象
            var Excmodel = Core.Reconciliation.WayBillException.Load(ID);
            //成本数据对象
            var Costmodel = Core.CostFlow.WayBillCost.Load(wayBillCostID);
            //执行方法
            Core.Reconciliation.WayBillException.DeleteException(wayBillCostID, Excmodel, Costmodel);

            return View();
        }

        //public JsonResult DeleteExceptionList(string expressNo)
        //{
        //    IList<WayBillCost> queryData = WayBillCost.GetCostByExpressNo(expressNo);
        //    var data = queryData.Select(u => new
        //    {
        //        ID = u.ID,
        //        ExpressNo = u.ExpressNo,        // 运单号
        //        BatchNO = u.BatchNO,            // 批次
        //        PostingTime = u.PostingTime.ToStringDate(),    // 收寄日期
        //        Weight = u.Weight,              // 重量
        //        SendAddress = u.SendAddress,    // 寄达地
        //        ProcessingFee = u.ProcessingFee,// 邮件处理费
        //        WayBillFee = u.WayBillFee,      // 邮资
        //        Product = u.Product             // 快递类型
        //    });

        //    //构造成Json的格式传递
        //    var result = new { iTotalRecords = queryData.Count, iTotalDisplayRecords = 10, data = data };
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region 提单对账
        /// <summary>
        /// 提单对账
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadBill()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadBillList(LoadBillReconciliationFilter filter)
        {
            IList<LoadBillStatistics> loadBillStatistics = new List<LoadBillStatistics>();
            DataTablesRequest parm = new DataTablesRequest(this.Request);    //处理对象
            int pageIndex = parm.iDisplayLength == 0 ? 0 : parm.iDisplayStart / parm.iDisplayLength;
            filter.PageIndex = pageIndex;    //页索引
            filter.PageSize = parm.iDisplayLength;    //页行数
            var DataSource = LoadBillReconciliation.GetByFilter(filter) as LRPageOfList<LoadBillReconciliation>;
            if (DataSource.Count > 0)
            {
                loadBillStatistics = Core.CostFlow.WayBillCost.GetLoadBillStatistics(DataSource.Select(a => a.LoadBillNum).ToList());
            }
            int i = parm.iDisplayLength * pageIndex;
            List<LoadBillReconciliation> queryData = DataSource.ToList();
            var data = queryData.Select(u =>
            {
                var CostExpressFee = loadBillStatistics.FirstOrDefault(a => a.LoadBillNum == u.LoadBillNum).Instance().WayBillFee;
                var CostOperateFee = loadBillStatistics.FirstOrDefault(a => a.LoadBillNum == u.LoadBillNum).Instance().ProcessingFee;
                return new
                {
                    Index = ++i, //行号
                    ID = u.ID,
                    CusName = u.CusName,//客户简称
                    LoadBillNum = u.LoadBillNum,//提单号
                    FeeWeight = u.FeeWeight,//提单重量
                    ExpressCount = u.ExpressCount,//提单包裹数量
                    CompletionTime = u.CompletionTime.ToStringDate(),//清关完成时间
                    GroundHandlingFee = u.GroundHandlingFee,//邮政地勤费
                    CostStoreFee = u.CostStoreFee,//邮政仓租
                    CostExpressFee = CostExpressFee,//邮政邮资
                    CostOperateFee = CostOperateFee,//邮件处理费
                    CostOtherFee = u.CostOtherFee,//邮政其他费用
                    CostTotalFee = u.GroundHandlingFee + u.CostStoreFee + CostExpressFee + CostOperateFee + u.CostOtherFee,//邮政总成本
                    CostStatus = u.CostStatus.ToChinese(),//邮政结算状态
                    InComeLoadFee = u.InComeLoadFee,//客户提货费
                    InComeStoreFee = u.InComeStoreFee,//客户仓租
                    InComeExpressFee = u.InComeExpressFee,//客户运费
                    InComeOperateFee = u.InComeOperateFee,//客户操作费
                    InComeOtherFee = u.InComeOtherFee,//其他费用
                    InComeTotalFee = u.InComeLoadFee + u.InComeStoreFee + u.InComeExpressFee + u.InComeOperateFee + u.InComeOtherFee,//总收入
                    InComeStatus = u.InComeStatus.ToChinese(),//结算状态
                    TotalGrossProfit = (u.InComeLoadFee + u.InComeStoreFee + u.InComeExpressFee + u.InComeOperateFee + u.InComeOtherFee) - (u.GroundHandlingFee + u.CostStoreFee + CostExpressFee + CostOperateFee + u.CostOtherFee),//总毛利
                    GrossProfitRate = Math.Round((u.InComeLoadFee + u.InComeStoreFee + u.InComeExpressFee + u.InComeOperateFee + u.InComeOtherFee) == 0 ? 0 : (1 - (u.GroundHandlingFee + u.CostStoreFee + CostExpressFee + CostOperateFee + u.CostOtherFee) / (u.InComeLoadFee + u.InComeStoreFee + u.InComeExpressFee + u.InComeOperateFee + u.InComeOtherFee)) * 100, 2),//毛利率
                    Status = u.Status,
                    IsReal = u.IsReal,
                    ExpressWeight = u.ExpressWeight,
                    IsAddMonthPayOff = u.IsAddMonthPayOff,
                    ReconcileDate = u.ReconcileDate.ToStringDate()
                };
            });
            //构造成Json的格式传递
            var result = new
            {
                iTotalRecords = DataSource.Count,
                iTotalDisplayRecords = DataSource.RecordTotal,
                data = data,
                UnusualCount = Core.Reconciliation.UnusualLoadBill.GetUnusualLoadBillCount(new UnusualLoadBillFilter() { CusName = filter.CusName, UnusualLoadBillNum = filter.LoadBillNum, CompletionSTime = filter.CompletionSTime, CompletionETime = filter.CompletionETime })
            };
            return Json(result);
        }

        /// <summary>
        /// 提单对账明细页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoadBillDetailed(string LoadBillNum)
        {
            ViewBag.LoadBillNum = LoadBillNum;
            ViewBag.UnusualWayBillCount = Core.Reconciliation.WayBillException.GetExceptionCount(new WayBillExceptionFilter() { LoadBillNum = LoadBillNum });
            return View(Core.Reconciliation.LoadBillReconciliation.GetByLoadBillNum(LoadBillNum));
        }

        /// <summary>
        /// 添加月结表
        /// </summary>
        /// <param name="no"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ActionResult AddMonthPayOff(DateTime payOffDate)
        {
            string strYear = payOffDate.Year.ToString();
            ViewBag.PreMonthStr = strYear + payOffDate.AddMonths(-1).Month.ToStringMonth();
            ViewBag.MonthStr = strYear + payOffDate.Month.ToStringMonth();
            ViewBag.NextMonthStr = strYear + payOffDate.AddMonths(1).Month.ToStringMonth();

            ViewBag.PreYearMonth = payOffDate.AddMonths(-1);
            ViewBag.YearMonth = payOffDate;
            ViewBag.NextYearMonth = payOffDate.AddMonths(1);
            return View();
        }

        /// <summary>
        /// 添加至月结表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isNeglect">是否忽视异常</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddMonthPayOff(List<MonthPayOffModel> data, bool isNeglect, DateTime payOffMonth)
        {
            string message = string.Empty;
            var dealdata = data.Select(a => new MonthPayOffData()
            {
                MonthPayTime = payOffMonth,
                ReconcileTime = a.ReconcileTime,
                PreTotalCostFee = a.PreTotalCostFee,
                TotalCostFee = a.TotalCostFee,
                PreInComeFee = a.PreInComeFee,
                InComeFee = a.InComeFee,
                TotalMargin = a.TotalMargin,
                LoadBillBy = LoadBillInCome.GetByLoadBillNum(a.LoadBillNum)
            }).ToList();
            if (MonthPayOff.AddMonthPay(isNeglect, dealdata, payOffMonth, out message))
            {
                return Json(new { IsSuccess = true, Message = string.Format("共计:{0}个提单添加到月结", data.Count) });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = message, IsPoint = message.Substring(0, 2) == "提醒" ? true : false });
            }
        }

        /// <summary>
        /// 提单对账异常
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ActionResult UnusualLoadBill(LoadBillReconciliationFilter filter)
        {
            UnusualLoadBillFilter f = new UnusualLoadBillFilter();
            f.CusName = filter.CusName;
            f.UnusualLoadBillNum = filter.LoadBillNum;
            f.CompletionSTime = filter.CompletionSTime;
            f.CompletionETime = filter.CompletionSTime;
            f.ReconcileETime = filter.ReconcileETime;
            f.ReconcileSTime = filter.ReconcileSTime;
            return View(f);
        }

        /// <summary>
        /// 提单对账异常数据请求
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ActionResult UnusualLoadBillList(UnusualLoadBillFilter filter)
        {
            DataTablesRequest parm = new DataTablesRequest(this.Request);    //处理对象
            int pageIndex = parm.iDisplayLength == 0 ? 0 : parm.iDisplayStart / parm.iDisplayLength;
            filter.PageIndex = pageIndex;    //页索引
            filter.PageSize = parm.iDisplayLength;    //页行数
            var DataSource = Core.Reconciliation.UnusualLoadBill.GetUnusualByFilter(filter);
            int i = parm.iDisplayLength * pageIndex;
            List<UnusualLoadBill> queryData = DataSource.ToList();
            var data = queryData.Select(u =>
            {
                return new
                {
                    Index = ++i, //行号
                    ID = u.ID,
                    CusName = u.CusName,//客户简称
                    LoadBillNum = u.LoadBillNum,//提单号
                    FeeWeight = u.FeeWeight,//提单重量
                    ExpressCount = u.ExpressCount,//提单包裹数量
                    CompletionTime = u.CompletionTime.ToStringDate(),//清关完成时间
                    GroundHandlingFee = u.GroundHandlingFee,//邮政地勤费
                    CostStoreFee = u.CostStoreFee,//邮政仓租
                    CostStatus = u.CostStatus.ToChinese(),//邮政结算状态
                    InComeLoadFee = u.InComeLoadFee,//客户提货费
                    InComeStoreFee = u.InComeStoreFee,//客户仓租
                    InComeStatus = u.InComeStatus.ToChinese(),//结算状态
                    Status = u.Status.ToChinese(),
                    ExpressWeight = u.ExpressWeight,
                    ReconcileDate = u.ReconcileDate.ToStringDate()
                };
            });
            //构造成Json的格式传递
            var result = new
            {
                iTotalRecords = DataSource.Count,
                iTotalDisplayRecords = DataSource.RecordTotal,
                data = data
            };
            return Json(result);
        }

        /// <summary>
        /// 异常提单对账详情
        /// </summary>
        /// <returns></returns>
        public ActionResult UnusualLoadBillDetail(int id)
        {
            var loadBillCost = LoadBillCost.Load(id).Instance();
            ViewBag.LoadBillInCome = LoadBillInCome.GetByLoadBillNum(loadBillCost.LoadBillNum).Instance();
            return View(loadBillCost);
        }

        /// <summary>
        /// 删除提单异常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteUnusualLoadBill(int id)
        {
            try
            {
                //提单成本对象
                LoadBillCost lc = LoadBillCost.Load(id);
                if (lc != null)
                {
                    lc.DeleteRecord();
                }
                return Json(new { IsSuccess = true, Message = "操作成功" });
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = true, Message = "操作失败" + ex });
            }
        }

        /// <summary>
        /// 设置提单正常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult SetNormal(int id)
        {
            string message = string.Empty;
            LoadBillCost lc = LoadBillCost.Load(id);
            if (lc != null && lc.SetNormal(out message))
            {
                return Json(new { IsSuccess = true, Message = "操作成功" });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = message });
            }
        }
        #endregion

        /// <summary>
        /// 提单费用导入
        /// </summary>
        /// <returns></returns>
        public ActionResult FeeImport()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult FeeUpload()
        {
            if (Request.Files["Filedata"] != null || Request.Files["Filedata"].ContentLength > 0)
            {
                try
                {
                    string message = string.Empty;
                    if (LoadBillInCome.ImportFee(Request.Files["Filedata"].InputStream, Request.Files["Filedata"].FileName, out message))
                    {
                        return Json(new { Success = true, Message = message });
                    }
                    return Json(new { Success = false, Message = message });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message });
                }
            }
            else
            {
                return Json(new { Success = false, Message = "请选择要上传的文件！" });
            }
        }

        /// <summary>
        /// 运单详细页
        /// </summary>
        /// <returns></returns>
        //public ActionResult WayBillDetailed(string ExpressNo)
        //{
        //    ViewBag.ExpressNo = ExpressNo;

        //    return View();
        //}

        /// <summary>
        /// 运单详细页
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WayBillDetailed(string ExpressNo)
        {
            ViewBag.ExpressNo = ExpressNo;
            var list = Core.Reconciliation.WayBillReconciliation.GetByFilter(new WayBillReconciliationFilter() { ExpressNo = ExpressNo, PageIndex = 0, PageSize = 1 }).ToList();

            return View(list);   //.FirstOrDefault()
        }

        /// <summary>
        /// 运单成本详细
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WayCostList(string ExpressNo)
        {

            var DataSource = WayBillCost.GetCostByExpressNo(ExpressNo);

            List<WayBillCost> queryData = DataSource.ToList(); //new List<WayBillCost>();
            //queryData.Add(DataSource);

            var data = queryData.Select(u => new
            {
                //ID = u.ID,
                ExpressNo = u.ExpressNo,   //运单号
                BatchNO = u.BatchNO, //提单号
                Weight = u.Weight / 1000,//重量
                PostingTime = u.PostingTime.ToStringDate(),//收寄日期
                SendAddress = u.SendAddress, //寄达地
                Product = u.Product, //快递类型           
                WayBillFee = u.WayBillFee, //运费
                ProcessingFee = u.ProcessingFee, //处理费
                WayBillMargins = 0         //其他费用
            });
            var result = new
            {
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 运单收入详细
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WayIncomeList(string ExpressNo)
        {

            var DataSource = WayBillReconciliation.GetWayBillInComeByExpressNo(ExpressNo);

            List<WayBillReconciliation> queryData = DataSource.ToList(); //new List<WayBillCost>();
            //queryData.Add(DataSource);

            var data = queryData.Select(u => new
            {
                //ID = u.ID,
                ExpressNo = u.ExpressNo,                        //运单号
                LoadBillNum = u.LoadBillNum,                    //提单号
                Weight = u.Weight,                              //重量
                PostingTime = u.CompletionTime.ToStringDate(),  //清关完成时间
                ReceiverProvince = u.ReceiverProvince,          //收件城市
                ExpressType = u.ExpressTypeget.ToChinese(),                 //快递类型     

                ExpressFee = u.ExpressFee,                      //运费
                OperateFee = u.OperateFee,                      //操作费
                WayBillMargins = 0                              //其他费用
            });
            var result = new
            {
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 异常详情
        /// </summary>
        /// <returns></returns>
        public ActionResult WayBillExceptionDetailed(string ExpressNo, string ID, string CostID)
        {
            ViewBag.ExpressNo = ExpressNo;
            ViewBag.ID = ID;
            ViewBag.CostID = CostID;
            return View(Core.Reconciliation.WayBillException.GetExceByID(ID).ToList());   //.FirstOrDefault()
        }

        /// <summary>
        /// 运单成本详细(根据异常表记入的运单成本ID去查询避免重复)
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult WayCostExByID(int CostID)
        {
            //var DataSource = Core.Reconciliation.WayBillException.GetCostByExpByID(CostID);
            //List<WayBillCost> queryData = DataSource.ToList(); //new List<WayBillCost>();         
            //var data = queryData.Select(u => new
            //{
            //    //ID = u.ID,
            //    ExpressNo = u.ExpressNo,   //运单号
            //    BatchNO = u.BatchNO, //提单号
            //    Weight = u.Weight,//重量
            //    PostingTime = u.PostingTime.ToStringDate(),//收寄日期
            //    SendAddress = u.SendAddress, //寄达地
            //    Product = u.Product, //快递类型           
            //    WayBillFee = u.WayBillFee, //运费
            //    ProcessingFee = u.ProcessingFee, //处理费
            //    WayBillMargins = 0         //其他费用
            //});
            //var result = new
            //{
            //    data = data
            //};
            //return Json(result, JsonRequestBehavior.AllowGet);

            var o = WayBillCost.Load(CostID);
            List<WayBillCost> li = new List<WayBillCost>();
            li.Add(o);
            var data = li.Select(u => new
            {
                //ID = u.ID,
                ExpressNo = u.ExpressNo,   //运单号
                BatchNO = u.BatchNO, //提单号
                Weight = u.Weight,//重量
                PostingTime = u.PostingTime.ToStringDate(),//收寄日期
                SendAddress = u.SendAddress, //寄达地
                Product = u.Product, //快递类型           
                WayBillFee = u.WayBillFee, //运费
                ProcessingFee = u.ProcessingFee, //处理费
                WayBillMargins = 0         //其他费用
            });
            var result = new
            {
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///提单异常数、运单异常数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExcBillCount()
        {
            var result = Core.Reconciliation.UnusualLoadBill.GetUnusualLoadBillCount();
            var data = new
            {
                UnusualCount = result[0],
                OrderCount = result[1]
            };
            return Json(data);
        }

        /// <summary>
        /// 运单异常数据修改
        /// </summary>
        /// <returns></returns>
        public ActionResult WayBillExceptionUpdate(int WayBillCostID, int id)
        {
            ViewBag.ID = WayBillCostID;
            ViewBag.ExcID = id;
            var result = WayBillCost.Load(WayBillCostID);
            return View(result);
        }

        /// <summary>
        /// 运单异常数据修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WayBillExceptionUpdate(WayBillCost costinfo, int ExcID, int WayBillCostID)
        {
            string msg = string.Empty;
            try
            {

                var model = Core.Reconciliation.WayBillException.Load(ExcID);
                Core.Reconciliation.WayBillException.UpdateException(costinfo, model, WayBillCostID);
                //ViewBag.Msg = string.Format("<script>$.dialog.alert('{0}')</script>", "修改成功！");
                ViewBag.Msg = string.Format(message, "修改成功", true, "1");
            }
            catch (Exception ex)
            {
                // ViewBag.Msg = string.Format("<script>$.dialog.alert('{0}')</script>", "修改失败！" + ex.Message);
                ViewBag.Msg = string.Format(message, "修改失败", false, "0");

            }
            return View();
        }
    }
}

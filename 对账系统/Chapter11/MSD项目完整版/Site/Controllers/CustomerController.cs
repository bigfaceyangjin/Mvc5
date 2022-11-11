using Core.Customer;
using Core.Filters;
using ProjectBase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    /// <summary>
    /// 客户控制器
    /// </summary>
    [Authorize]
    public class CustomerController : Controller
    {
        private string message = "<script>frameElement.api.opener.hidePublishWin('{0}', '{1}','{2}'); </script>"; //消息，是否关闭弹出窗，是否停留在当前分页（0，1）

        #region 客户管理主页
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 客户列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult List(CustomerFilter filter)
        {
            filter.PageSize = int.MaxValue;
            var dataSource = CustomerInfo.GetByFilter(filter);

            List<CustomerInfo> queryData = dataSource.ToList();

            var data = queryData.Select(u => new
            {
                ID = u.ID,
                CusCode = u.CusCode,
                CusName = u.CusName,
                BusssinessType = u.BusssinessType.GetDescription(false),
                Balance = u.Balance,
                CreditAmount = u.CreditAmount,
                Status = u.Status.GetDescription(false),
                Country = u.Country,
                CompanyName = u.CompanyName,
                Delivery = GetDeliveryList(u.ExpressCurInfoBy)

            });

            //构造成Json的格式传递
            var result = new { iTotalRecords = queryData.Count, iTotalDisplayRecords = 10, data = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有收货商名称
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string GetDeliveryList(IList<ExpressCurInfo> list)
        {
            StringBuilder result = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                foreach (ExpressCurInfo ex in list)
                {
                    if (result.Length > 0)
                    {
                        result.Append("， ");
                    }
                    result.Append(ex.DeliveryName);
                }
            }
            return result.ToString();
        }
        #endregion

        #region 添加客户
        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddCustomer()
        {
            ViewBag.Title = "添加客户";
            return View();
        }

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCustomer(CustomerInfo info)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    info.Save();
                    msg = "添加客户成功。";
                }
                catch (Exception ex)
                {
                    msg = "添加客户失败！" + ex.Message;
                    ViewBag.Msg = string.Format(message, msg, false, "1");
                }
                ViewBag.Msg = string.Format(message, msg, true, "0");
            }
            return View();
        }
        #endregion

        #region 修改客户
        /// <summary>
        /// 修改客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UpdateCustomer(int id)
        {
            ViewBag.Title = "修改客户";
            var result = CustomerInfo.Load(id);

            return View(result);
        }

        /// <summary>
        /// 修改客户
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateCustomer(CustomerInfo info)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    CustomerInfo model = CustomerInfo.Load(info.ID);
                    model.CusCode = info.CusCode;
                    model.CusName = info.CusName;
                    model.Phone = info.Phone;
                    model.Tel = info.Tel;
                    model.Email = info.Email;
                    model.Fax = info.Fax;
                    model.Country = info.Country;
                    model.Address = info.Address;
                    model.CompanyName = info.CompanyName;
                    model.BusssinessType = info.BusssinessType;
                    model.Status = info.Status;
                    model.Update();
                    msg = "修改客户成功。";
                }
                catch (Exception ex)
                {
                    msg = "修改客户失败！" + ex.Message;
                    ViewBag.Msg = string.Format(message, msg, false, "1");
                }
                ViewBag.Msg = string.Format(message, msg, true, "0");
            }
            return View();
        }
        #endregion

        #region 客户匹配
        public ActionResult DeliveryMatching(int id)
        {
            ViewBag.Title = "收货商匹配";
            ViewBag.CustomerId = id;
            return View();
        }

        /// <summary>
        /// 快件客户信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeliveryList()
        {
            //filter.PageSize = int.MaxValue;
            var dataSource = ExpressCurInfo.GetAll();

            List<ExpressCurInfo> queryData = dataSource.Where(e => e.IsMatch == false).ToList();
            var data = queryData.Select(u => new
            {
                ID = u.ID,
                DeliveryID = u.DeliveryID,
                DeliveryName = u.DeliveryName,
                AccountName = u.AccountName
            });

            //构造成Json的格式传递
            var result = new { iTotalRecords = queryData.Count, iTotalDisplayRecords = 10, data = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddMatching(int customerId, int deliveryId)
        {
            string msg = string.Empty;
            bool result = true;
            try
            {
                CustomerInfo curCus = CustomerInfo.Load(customerId);
                ExpressCurInfo curExp = ExpressCurInfo.Load(deliveryId);
                curExp.MatchCustomer(curCus);
                msg = "匹配成功。";
                result = true;
            }
            catch (Exception ex)
            {
                msg = "匹配失败！" + ex.Message;
                result = false;
            }
            return Json(new { Msg = msg, Result = result ? "True" : "False" });
        }
        /// <summary>
        /// 批量匹配
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="deliveryId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddMatchingList(int customerId, string ListID)
        {
            string[] str = ListID.Split(new char[] { ',' }); //

            string msg = string.Empty;
            bool result = true;
            try
            {
                for (int i = 0; i < str.Length; i++)
                {
                    CustomerInfo curCus = CustomerInfo.Load(customerId);
                    ExpressCurInfo curExp = ExpressCurInfo.Load(Convert.ToInt32(str[i]));
                    curExp.MatchCustomer(curCus);                  
                }
                msg = "匹配成功。";
                result = true;
            }
            catch (Exception ex)
            {
                msg = "匹配失败！" + ex.Message;
                result = false;
            }
            return Json(new { Msg = msg, Result = result ? "True" : "False" });        
        }
        #endregion
    }
}

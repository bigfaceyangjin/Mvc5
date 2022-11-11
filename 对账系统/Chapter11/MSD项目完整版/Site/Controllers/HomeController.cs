using Core.UserModule;
using ProjectBase.Utils.Entities;
using System.Web.Mvc;

namespace MSDSite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "欢迎使用财务模块";

            return View();
        }
        public ActionResult Top()
        {
            ViewBag.UserName = UserAccount.GetByAccount(User.Identity.Name).Instance().RealName;
            ViewBag.AvailableBalance = "8888.00";
            return View();
        }
        public ActionResult Left()
        {
            return View();
        }
        public ActionResult Right()
        {
            return View();
        }
        public ActionResult Main()
        {
            //SysInfo _SysInfo = new SysInfo().GetSysInfo();
            return RedirectToAction("MonthPayOff","Statistical");
        }
    }
}

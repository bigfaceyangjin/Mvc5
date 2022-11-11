using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Site.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            //获取客户存储的其他信息(HttpContext.User.Identity as System.Web.Security.FormsIdentity).Ticket.UserData;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string userName, string passWord, bool? isRemember)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (Core.UserModule.UserAccount.LoginValid(userName, passWord, out message))
                {
                    FormsAuthenticationTicket ticket;
                    if (isRemember.HasValue&&isRemember.Value)
                    {
                        ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout), false, "admin");
                    }
                    else
                    {
                        ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddHours(2), false, "admin");
                    }
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Message = message;
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View("Index");
        }

        [AllowAnonymous]
        public ActionResult LoginOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}

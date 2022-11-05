using Eonup.Framework.ExtensionMethod;
using Eonup.Mvc5.Filter;
using Eonup.Mvc5.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Eonup.Mvc5.Utility.UserManager;

namespace Eonup.Mvc5.Controllers
{
    public class SixController : Controller
    {
        // GET: Six
        public ActionResult Index()
        {
            return View();
        }
		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}
		/// <summary>
		/// 用户登录
		/// </summary>
		/// <param name="name"></param>
		/// <param name="password"></param>
		/// <param name="txtVerify"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Login(string name, string password, string txtVerify)
		{
			LoginResult result = base.HttpContext.Login(name, password, txtVerify);
			if (result == LoginResult.Success)
			{
				if (base.HttpContext.Session["CurrentUrl"] != null)
				{
					return Redirect(base.HttpContext.Session["CurrentUrl"].ToString());
				}
				return Redirect("/Fifth/Index");
			}
			else {
				ModelState.AddModelError("failed", result.GetReMark());
				return View();
			}
		}
		/// <summary>
		/// 异步登录验证
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AjaxLogin()
		{
			if (ModelState.IsValid)
			{
				//账号、密码、验证码
				string Name = base.HttpContext.Request.Form["Name"];
				string Password = base.HttpContext.Request.Form["Password"];
				string Verify = base.HttpContext.Request.Form["txtVerify"];
				//验证结果
				LoginResult rs = base.HttpContext.Login(Name, Password, Verify);
				string url = "/Fifth/Index";
				if (base.HttpContext.Session["CurrentUrl"] != null)
				{
					url = base.HttpContext.Session["CurrentUrl"].ToString();
				}
				return Json(new { result = (int)rs, msg = rs.GetReMark(),Url=url});
			}
			else {
				ModelState.AddModelError("failed", "ModelState验证失败！");
				return View("Login");
			}
		}
		/// <summary>
		/// 返回验证码图片
		/// 将验证码内容存放于上下文Session中
		/// </summary>
		/// <returns></returns>
		public ActionResult VerifyCode()
		{
			Bitmap bitmap= VerifyCodeHelper.CreateVerifyCode(out string code);//生成验证码图片
			base.HttpContext.Session["VerifyCode"] = code;//验证码文字内容 Session识别用户 为单一用户有效
			MemoryStream ms = new MemoryStream();
			bitmap.Save(ms, ImageFormat.Gif);//将图片基于gif的形式存放于内存流中返回
			return File(ms.ToArray(),"image/gif");
		}
		/// <summary>
		/// 创建验证码图片并将其存放在响应流中
		/// 将验证码内容存放于上下文Session中
		/// </summary>
		public void Verify()
		{
			Bitmap bitmap = VerifyCodeHelper.CreateVerifyCode(out string code);
			base.HttpContext.Session["verify"] = code;
			bitmap.Save(Response.OutputStream, ImageFormat.Gif);
			base.Response.ContentType = "image/gif";
		}
		/// <summary>
		/// 清除当前登录用户的 Session 或 Cookie
		/// </summary>
		public ActionResult LogOut()
		{
			base.HttpContext.Session.Remove("CurrentUser");
			HttpCookie ck = base.HttpContext.Request.Cookies["CurrentUser"];
			if (ck != null)
			{
				ck.Expires = DateTime.Now.AddMinutes(-1);
				base.HttpContext.Response.Cookies.Add(ck);
			}
			return RedirectToAction("Login");
		}
	}
}
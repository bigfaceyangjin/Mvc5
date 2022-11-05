using Eonup.Business.Interface;
using Eonup.EF.Model;
using Eonup.Framework.Unity;
using Eonup.Mvc5.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
namespace Eonup.Mvc5.Utility
{
	public static class UserManager
	{
		/// <summary>
		/// 登录验证
		/// </summary>
		/// <param name="context">存放于上下文中的正确Session验证码</param>
		/// <param name="name">账号</param>
		/// <param name="password">密码</param>
		/// <param name="verify">输入的验证码</param>
		/// <returns></returns>
		public static LoginResult Login(this HttpContextBase context,string name, string password,string verify)
		{
			if (context.Session["VerifyCode"] != null && !string.IsNullOrEmpty(context.Session["VerifyCode"].ToString()) && context.Session["VerifyCode"].ToString().Equals(verify, StringComparison.CurrentCultureIgnoreCase))
			{
				using (IUserService service = ContainerFactory.CreateContainer().Resolve<IUserService>())
				{
					User user= service.Set<User>().Where(x => x.Name.Equals(name) || x.Account.Equals(name) || x.Email.Equals(name)).FirstOrDefault();
					if (user == null)
					{
						return LoginResult.NoUser;
					}
					else if (!user.Password.Equals(MD5Encrypt.Encrypt(password)))
					{
						return LoginResult.ErrorPwd;
					}
					else if (user.State == 1)
					{
						return LoginResult.Frozen;
					}
					else {
						#region Session 存当前登录用户
						CurrentUser currentUser = new CurrentUser() {
							Id=user.Id,
							Name=user.Name,
							Email=user.Email,
							Account=user.Account,
							Password=user.Password,
							LoginTime=DateTime.Now
						};
						context.Session["CurrentUser"]= currentUser;
						context.Session.Timeout = 3;
						#endregion
						#region Cookie
						HttpCookie cookie = new HttpCookie("curUser");
						cookie.Value = JsonConvert.SerializeObject(currentUser);
						cookie.Expires = DateTime.Now.AddMinutes(1);
						context.Response.Cookies.Add(cookie);//一定要输出
						#endregion
						return LoginResult.Success;
					}
				}
			}
			else {
				return LoginResult.ErrorVerify;
			}
		}

		public static void UserLogout(this HttpContextBase context)
		{
			
		}
		public enum LoginResult {
			[Remark("登录成功")]
			Success=0,
			[Remark("用户不存在")]
			NoUser=1,
			[Remark("密码错误")]
			ErrorPwd=2,
			[Remark("验证码错误")]
			ErrorVerify=3,
			[Remark("账号已冻结")]
			Frozen =4
		}
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eonup.Mvc5.Models
{
	public class CurrentUser
	{
		public int? Id { get; set; }
		[Display(Name="账户")]
		[Required]
		public string Name { get; set; }
		[Display(Name="账号")]
		public string Account { get; set; }
		[Display(Name="邮箱")]
		public string Email { get; set; }
		[Display(Name="密码")]
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Display(Name="登录时间")]
		public DateTime? LoginTime { get; set; }
	}
}
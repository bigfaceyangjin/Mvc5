using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Eonup.Mvc5.Utility
{
	/// <summary>
	/// 备注
	/// </summary>
	public class RemarkAttribute:Attribute
	{
		private string _remark = string.Empty;
		public RemarkAttribute(string remark)
		{
			this._remark = remark;
		}
		public string Remark {
			get {
				return _remark;
			}
			set {
				_remark = value;
			}
		}
	}
	public static class RemarkExtension {
		/// <summary>
		/// 获取该值的RemarkAttribute特性值
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public static string GetReMark(this Enum e)
		{
			Type type= e.GetType();//枚举类型  例如：类型:LoginResult  值：Success
			FieldInfo field= type.GetField(e.ToString());//获取此枚举类型中的这个字段 
														 //判断这个类型中的这个字段有没有RemarkAttribute特性 如果有 就返回Remark值
			if (field.IsDefined(typeof(RemarkAttribute), true))
			{
				RemarkAttribute attri = (RemarkAttribute)field.GetCustomAttribute(typeof(RemarkAttribute), true);
				return attri.Remark;
			}
			return e.ToString();
		}
	}
}
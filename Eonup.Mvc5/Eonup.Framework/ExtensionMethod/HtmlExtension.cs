using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Eonup.Framework.ExtensionMethod
{
	public static class HtmlExtension
	{
		/// <summary>
		/// 自定义一个Submit控件 @Html.Submit("value","class")
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <param name="value">控件文本值</param>
		/// <param name="defaultClass">默认样式</param>
		/// <returns></returns>
		public static MvcHtmlString Submit(this HtmlHelper htmlHelper,string value,string defaultClass="btn btn-default")
		{
			var builder= new TagBuilder("input");
			builder.MergeAttribute("type", "submit");
			builder.MergeAttribute("value", value);
			builder.MergeAttribute("class", defaultClass);
			builder.ToString(TagRenderMode.EndTag);
			return MvcHtmlString.Create(builder.ToString());
		}
		public static MvcHtmlString Button(this HtmlHelper htmlHelper, string value,string id, string defaultClass = "btn btn-default")
		{
			var builder = new TagBuilder("input");
			builder.MergeAttribute("type", "button");
			builder.MergeAttribute("id", id);
			builder.MergeAttribute("value", value);
			builder.MergeAttribute("class", defaultClass);
			builder.ToString(TagRenderMode.EndTag);
			return MvcHtmlString.Create(builder.ToString());
		}
	}
}

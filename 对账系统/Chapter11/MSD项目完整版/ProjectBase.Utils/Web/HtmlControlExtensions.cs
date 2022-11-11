using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.ComponentModel;
using ProjectBase.Utils.Entities;
using System.Text.RegularExpressions;

namespace ProjectBase.Utils.Web
{
    public static class HtmlControlExtensions
    {
        public static string DropDownList<T>(this HtmlHelper html, string name) where T : struct, IConvertible
        {
            var list = Enum.GetNames(typeof(T)).Select(p => new SelectListItem { Text = ((Enum)(object)p.Parse<T>()).ToChinese(), Value = p.Parse<T>().ToString() }).ToList();
            return html.DropDownList(name, list).ToString();
        }

        public static string DropDownList<T>(this HtmlHelper html, string name,T? select) where T : struct, IConvertible
        {
            var list = Enum.GetNames(typeof(T)).Select(p => new SelectListItem { Text = ((Enum)(object)p.Parse<T>()).ToChinese(), Value = p.Parse<T>().ToString(), Selected = p.Parse<T>().Equals(select.Value) }).ToList();
            return html.DropDownList(name, list).ToString();
        }

        public static string DropDownList<T>(this HtmlHelper html, string name, string label) where T : struct, IConvertible
        {
            var list = Enum.GetNames(typeof(T)).Select(p => new SelectListItem { Text = ((Enum)(object)p.Parse<T>()).ToChinese(), Value = p.Parse<T>().ToString() }).ToList();
            return html.DropDownList(name, list, label).ToString();
        }

        public static string DropDownList<T>(this HtmlHelper html, string name, int index, SelectListItem item) where T : struct, IConvertible
        {
            var list = Enum.GetNames(typeof(T)).Select(p => new SelectListItem { Text = ((Enum)(object)p.Parse<T>()).ToChinese(), Value = p.Parse<T>().ToString() }).ToList();
            list.Insert(index, item);
            return html.DropDownList(name, list).ToString();
        }

        public static string DropDownListInt<T>(this HtmlHelper html, string name) where T : struct, IConvertible
        {
            var list = Enum.GetNames(typeof(T)).Select(p => new SelectListItem { Text = ((Enum)(object)p.Parse<T>()).ToChinese(), Value = Convert.ToInt32(p.Parse<T>()).ToString() }).ToList();
            return html.DropDownList(name, list).ToString();
        }

        public static void Include(this HtmlHelper html, string path)
        {
            try
            {
                html.ViewContext.HttpContext.Response.Write(System.IO.File.ReadAllText(html.ViewContext.HttpContext.Server.MapPath(path)));
            }
            catch 
            { }
        }

        public static string ConverToHtml(this HtmlHelper html, string text)
        {
            if (text == null)
                return text;
            return text.Replace(" ", "&nbsp;")
                .Replace(Environment.NewLine, "<br/>")
                .Replace("\n", "<br/>")
                .Replace("\r", "")
                ;
        }

        public static string CheckBoxGroup(this HtmlHelper html, string name,IList<SelectListItem> items)
        {
            StringBuilder sb = new StringBuilder("<ul>");
            for (int i=0;i<items.Count;i++)
            {
                sb.AppendFormat("<li><input type=\"checkbox\" name=\"{0}\" id=\"{1}\" value=\"{2}\" {3}/></li><label for=\"{1}\">{4}</label>"
                    , name, name + "_" + i, items[i].Value, items[i].Selected ? "checked=\"checked\"" : "", items[i].Text);
                sb.AppendLine();
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
    }
}

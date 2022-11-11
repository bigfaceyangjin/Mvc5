using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ProjectBase.Utils.Entities
{
    public static class MethodExtensions
    {
        public static Action<T> Fix<T>(this HtmlHelper helper, Func<Action<T>, Action<T>> func)
        {          
            return x => func(Fix(helper, func))(x);
        }

        public static string IsTrue(this HtmlHelper helper,bool condition, string html){
            if (condition)
                return html;
            else
                return "";
        }
    }
}

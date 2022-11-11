using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBHelper.Extend
{
    public static class FormatExtend
    {
        public static string ToDateStr(this DateTime? o)
        {
            if (o.HasValue)
            {
                return string.Format("'{0}'",o.Value.ToString());
            }
            else
            {
                return "null";
            }
        }

        public static string ToDateStr(this DateTime o)
        {
            return string.Format("'{0}'",o.ToString());
        }

        public static string ToSqlStr(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "null";
            }
            else
            {
                return string.Format("'{0}'", s.Replace("'", "''"));
            }
        }

        public static string ToSqlStr(this int? i)
        {
            if (i.HasValue)
            {
                return i.ToString();
            }
            else
            {
                return "null";
            }
        }
    }
}

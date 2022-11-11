using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBase.Utils.Web
{
    public class SiteLocation
    {
        public string Text { get; set; }

        public string Url { get; set; }

        public bool IsLink { get { return !string.IsNullOrEmpty(Url); } }
    }
}

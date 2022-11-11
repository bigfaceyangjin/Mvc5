using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace ProjectBase.Data
{
    public class LogManager
    {
        public static ILog Logger
        {
            get
            {
                return Nested.logger;
            }
        }

        private class Nested
        {
            static Nested() { }
            internal static readonly ILog logger = log4net.LogManager.GetLogger(typeof(LogManager));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectBase.Utils;

namespace ProjectBase.Data
{
    public static class TestExtension
    {
        public static T Is<T>(this Object obj) where T : class
        {
            Check.Assert(obj is T);
            return (T)obj;
        }
    }
}

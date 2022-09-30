using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eonup.Framework
{
	public class PageResult<T>
	{
		public int pageIndex { get; set; }
		public int pageSize { get; set; }
		public int totalCount { get; set; }
		public List<T> dataList { get; set; }
	}
}
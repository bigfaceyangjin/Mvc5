using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eonup.Mvc5.PipeLine
{
	public class CustomeModule : IHttpModule
	{
		public void Init(HttpApplication context)
		{
			context.BeginRequest += (s, e) => { context.Response.Write($"<h3>来自CustomeModule的处理，{DateTime.Now.ToString()} BeginRequest</h3>"); };
		}


		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
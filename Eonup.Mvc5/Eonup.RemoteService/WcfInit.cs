using Eonup.Wcf.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Eonup.RemoteService
{
	public class WcfInit
	{
		private static List<ServiceHost> list = null;
		public static void OpenService()
		{
			list = new List<ServiceHost>();
			list.Add(new ServiceHost(typeof(SearchService)));
			foreach (ServiceHost host in list)
			{
				host.Opened += (s, e) => { Console.WriteLine($"服务以及打开 {host.Description}"); };
				host.Open();
			}
		}

		public static void CloseService()
		{
			foreach (ServiceHost host in list)
			{
				host.Closed += (o, e) => { Console.WriteLine($"{host.Description}已经关闭"); };
				host.Close();
				host.Abort();
			}
		}
		
	}
}

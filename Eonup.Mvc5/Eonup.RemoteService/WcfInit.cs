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
		public static void OpenService()
		{
			List<ServiceHost> list = new List<ServiceHost>();
			list.Add(new ServiceHost(typeof(SearchService)));
			foreach (ServiceHost host in list)
			{
				host.Opened += (s, e) => {  };
				host.Open();
			}
		}

		private static void Host_Opened(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}

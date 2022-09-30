using Eonup.Framework.Log4;
using Eonup.Remote.Interface;
using Eonup.Remote.Service.RemoteSearchService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonup.Remote.Service
{
	public class SearchService: Eonup.Remote.Interface.ISearchService
	{
		Logger loger = new Logger(typeof(SearchService));
		public string Query(List<int> Ids)
		{
			SearchServiceClient client = null;
			try
			{
				loger.Info($"{this.GetType().Name}");
				client = new SearchServiceClient();
				string result = client.Query(Ids);
				return result;
			}
			catch (Exception ex)
			{
				if (client != null)
					client.Abort();
				throw new Exception(ex.Message);
			}
		}
	}
}

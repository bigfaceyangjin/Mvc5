using Eonup.Wcf.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonup.Wcf.Service
{
    public class SearchService: ISearchService
	{
		public string Query(List<int> Ids)
		{
			return JsonConvert.SerializeObject(new { Id="10086",Name="皮特熔岩",Time=DateTime.Now
			});
		}
    }
}

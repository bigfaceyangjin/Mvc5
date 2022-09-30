using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonup.Remote.Interface
{
	public interface ISearchService
	{
		string Query(List<int> Ids);
	}
}

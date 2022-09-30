using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Eonup.Wcf.Interface
{
	[ServiceContract]
    public interface ISearchService
    {
		[OperationContract]
		string Query(List<int> Ids);
    }
}

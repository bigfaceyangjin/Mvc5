using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonup.Business.Interface
{
	public interface IProductService:IBaseService
	{
		IQueryable<T> GetDropList<T>() where T : class;
	}
}

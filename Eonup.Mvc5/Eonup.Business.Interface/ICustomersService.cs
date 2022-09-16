using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eonup.EF.Model.Model;
namespace Eonup.Business.Interface
{
	public interface ICustomersService:IBaseService
	{
		void AddCategory(Category category);
	}
}

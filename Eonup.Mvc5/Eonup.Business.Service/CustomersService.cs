using Eonup.Business.Interface;
using Eonup.EF.Model.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonup.Business.Service
{
	public class CustomersService : BaseService, ICustomersService
	{
		public CustomersService(DbContext context) : base(context) {

		}
		public void AddCategory(Category category)
		{
			Console.WriteLine("AddCategory");
		}
	}
}

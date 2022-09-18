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
	public class CompanyService: BaseService,ICompanyService
	{
		public CompanyService(DbContext context) : base(context)
		{

		}

		
	}
}

using Eonup.Business.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonup.Business.Service
{
	public class UserService:BaseService,IUserService
	{
		public UserService(DbContext context) : base(context)
		{

		}
	}
}

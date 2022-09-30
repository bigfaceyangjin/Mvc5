using Eonup.Business.Interface;
using Eonup.EF.Model.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Eonup.Business.Service
{
	public class ProductService:BaseService, IProductService
	{
		public ProductService(DbContext context) : base(context)
		{
		}
		/// <summary>
		/// 供应商下拉框
		/// </summary>
		/// <returns></returns>
		public IQueryable<T> GetDropList<T>() where T : class
		{
			return base._context.Set<T>();
		}
	}
}

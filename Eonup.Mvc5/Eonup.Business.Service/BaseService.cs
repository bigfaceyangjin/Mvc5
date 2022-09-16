using Eonup.Business.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonup.Business.Service
{
	/// <summary>
	/// BLL基类、泛型重用
	/// </summary>
	public abstract class BaseService : IBaseService
	{
		#region IDentity
		protected DbContext _context = null;
		/// <summary>
		/// 构造函数注入EF上下文
		/// </summary>
		/// <param name="context"></param>
		public BaseService(DbContext context)
		{
			this._context = context;
		}
		#endregion
		public T Find<T>(string Id) where T : class
		{
			return this._context.Set<T>().Find(Id);
		}

		public IQueryable<T> Set<T>() where T : class
		{
			return this._context.Set<T>();
		}

		public  void Dispose()
		{
			if (this._context != null)
			{
				this._context.Dispose();
			}
		}
	}
}

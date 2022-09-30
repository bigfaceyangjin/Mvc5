using Eonup.Business.Interface;
using Eonup.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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
		public T Find<T>(int Id) where T : class
		{
			return this._context.Set<T>().Find(Id);
		}

		public IQueryable<T> Set<T>() where T : class
		{
			return this._context.Set<T>();
		}
		/// <summary>
		/// 数据列表 分页数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public PageResult<T> Query<T>(Expression<Func<T, bool>> lambdaWhere, Expression<Func<T, decimal>> orderby,int pageIndex,int pageSize,bool Asc=true) where T : class
		{
			var list= this.Set<T>();
			int totalCount;
			if (lambdaWhere != null)
			{
				list = list.Where(lambdaWhere);
				totalCount = list.Count(lambdaWhere);
			}
			else {
				totalCount = list.Count();
			}
			if (Asc)
			{
				list = list.OrderBy(orderby);
			}
			else {
				list = list.OrderByDescending(orderby);
			}
			PageResult<T> result = new PageResult<T>() {
				pageIndex = pageIndex,
				pageSize = pageSize,
				dataList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
				totalCount = totalCount
			};
			return result;
		}

		public T Insert<T>(T t) where T : class
		{
			this._context.Set<T>().Add(t);
			this._context.SaveChanges();
			return t;
		}
		public void Update<T>(T t) where T:class
		{
			this._context.Set<T>().Attach(t);//将实体附加到数据库 未修改状态
			this._context.Entry<T>(t).State = EntityState.Modified;//修改状态
			this._context.SaveChanges();
		}
		public void Delete<T>(T t) where T : class
		{
			this._context.Set<T>().Attach(t);
			this._context.Set<T>().Remove(t);
			this._context.SaveChanges();
		}
		/// <summary>
		/// 获取一级类
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public IEnumerable<T> GetChildList<T>(Expression<Func<T, bool>> func,string key = "firstChild") where T : class
		{
			Dictionary<string, List<T>> dic = new Dictionary<string, List<T>>();
			if (dic.Keys.Contains(key))
			{
				return dic[key];
			}
			else
			{
				dic.Add(key, this.Set<T>().Where(func).ToList());
				return dic[key];
			}
		}

		/// <summary>
		/// 缓存表所有数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IEnumerable<T> CacheAllCategory<T>()where T:class
		{
			Dictionary<string, List<T>> dic = new Dictionary<string, List<T>>();
			string key = "allCategory";
			if (!dic.Keys.Contains(key))
			{
				dic.Add(key,this.Set<T>().ToList());
			}
			return dic[key];
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

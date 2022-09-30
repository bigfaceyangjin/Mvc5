using Eonup.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eonup.Business.Interface
{
	public interface IBaseService:IDisposable
	{
		/// <summary>
		/// 根据Id查询单个实体
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="Id"></param>
		/// <returns></returns>
		T Find<T>(int Id) where T : class;
		/// <summary>
		/// 单表查询
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IQueryable<T> Set<T>() where T : class;
		/// <summary>
		/// 分页数据列表
		/// </summary>
		/// <typeparam name="T">查询实体</typeparam>
		/// <param name="lambdaWhere">查询条件</param>
		/// <param name="orderby">排序条件</param>
		/// <param name="pageIndex">页索引</param>
		/// <param name="pageSize">页容量</param>
		/// <param name="Asc">升/降序</param>
		/// <returns></returns>
		PageResult<T> Query<T>(Expression<Func<T, bool>> lambdaWhere, Expression<Func<T, decimal>> orderby, int pageIndex, int pageSize, bool Asc = true) where T : class;

		/// <summary>
		/// 新增实体
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		/// <returns></returns>
		T Insert<T>(T t) where T:class;

		/// <summary>
		/// 修改实体
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		void Update<T>(T t) where T : class;

		/// <summary>
		/// 删除实体
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		void Delete<T>(T t) where T : class;
		IEnumerable<T> GetChildList<T>(Expression<Func<T, bool>> func, string key = "firstChild") where T : class;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
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
		T Find<T>(string Id) where T : class;
		/// <summary>
		/// 单表查询
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IQueryable<T> Set<T>() where T : class;
	}
}

using Eonup.Business.Interface;
using Eonup.EF.Model;
using Eonup.Framework;
using Eonup.Remote.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonup.Business.Service
{
	/// <summary>
	/// 商品服务
	/// </summary>
	public class CommodityService:BaseService, ICommodityService
	{
		public CommodityService(DbContext context) : base(context)
		{

		}
		public IEnumerable<Commodity> GetProduct(List<int> cateIdList,string searchString)
		{
			return base.Set<Commodity>().Where(x=>cateIdList==null ? true : (cateIdList.Contains(x.CategoryId)) 
			&&x.Title.Contains(searchString)).ToList();
		}
		public void Show()
		{
			throw new Exception("这里是 CommodityService Show Exception");
		}
	}
}

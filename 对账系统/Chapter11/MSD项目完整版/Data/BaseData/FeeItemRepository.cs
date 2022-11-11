using Core.BaseData;
using Core.BaseData.Repositories;
using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BaseData
{
    /// <summary>
    /// 费用项DAL
    /// </summary>
    public class FeeItemRepository : AbstractNHibernateDao<FeeItem, int>, IFeeItemRepository
    {
    }
}

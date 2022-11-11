using Core.CostFlow;
using Core.CostFlow.Repositories;
using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.CostFlow
{
    /// <summary>
    /// 身份证验证流水
    /// </summary>
    public class IdentityCardDetailRepository : AbstractNHibernateDao<IdentityCardDetail, int>, IIdentityCardDetailRepository
    {
    }
}

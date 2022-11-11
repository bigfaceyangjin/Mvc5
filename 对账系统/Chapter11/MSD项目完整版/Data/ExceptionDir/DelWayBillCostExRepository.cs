using Core.ExceptionDir;
using Core.ExceptionDir.Repositories;
using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ExceptionDir
{
    public class DelWayBillCostExRepository : AbstractNHibernateDao<DelWayBillCostEx, int>, IDelWayBillCostExRepository
    {
    }
}

using Core.ExceptionDir;
using Core.ExceptionDir.Repositories;
using ProjectBase.Data;
/* ==============================================================================
   * 功能描述：DelLoadBillCostExRepository  
   * 创 建 者：Zouqj
   * 创建日期：2015/6/26 11:03:25
   ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ExceptionDir
{
    public class DelLoadBillCostExRepository : AbstractNHibernateDao<DelLoadBillCostEx, int>, IDelLoadBillCostExRepository
    {

    }
}

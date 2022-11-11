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
    /// 币种DAL
    /// </summary>
    public class CurrencyRepository : AbstractNHibernateDao<Currency, int>, ICurrencyRepository
    {
    }
}

using Core.Filters;
using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Receivable.Repositories
{
    public interface IMonthPayOffRepository : IDao<MonthPayOff, int>
    {
        IList<MonthPayOff> GetByPayOffMonth(DateTime dt);
    }
}

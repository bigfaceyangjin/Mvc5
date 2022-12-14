using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Receivable.Repositories
{
    public interface IMonthPayOffDetailRepository : IDao<MonthPayOffDetail, int>
    {
        IList<MonthPayOff> GetPayOffMonth(IList<int> loadBillIDs);
        MonthPayOffDetail GetByLoadBillID(int loadBillID);
    }
}

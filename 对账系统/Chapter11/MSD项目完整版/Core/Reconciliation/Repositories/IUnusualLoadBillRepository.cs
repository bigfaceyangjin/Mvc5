using ProjectBase.Data;
using ProjectBase.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reconciliation.Repositories
{
    public interface IUnusualLoadBillRepository : IDao<UnusualLoadBill, int>
    {
        long GetUnusualLoadBillCount(ParameterFilter filter);
        IPageOfList<UnusualLoadBill> GetUnusualByFilter(ParameterFilter filter);
        List<long> GetUnusualLoadBillCount();
    }

    public interface ILoadBillCostRepository : IDao<LoadBillCost, int>
    {
        IList<LoadBillCost> GetByLoadBillNum(string loadBillNum);
    }
}

using ProjectBase.Data;
/* ==============================================================================
   * 功能描述：IWayBillReconciliationRepository  
   * 创 建 者：Zouqj
   * 创建日期：2015/6/3 12:34:00
   ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reconciliation.Repositories
{
    public interface IWayBillReconciliationRepository : IDao<WayBillReconciliation, int>
    {
        /// <summary>
        /// 根据运单号获取收入详细
        /// </summary>
        /// <param name="ExpressNo"></param>
        /// <returns></returns>
        IList<WayBillReconciliation> GetWayBillInComeByExpressNo(string ExpressNo);
    }
}

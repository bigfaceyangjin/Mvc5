using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Customer.Repositories
{
    public interface IExpressCurInfoRepository : IDao<ExpressCurInfo, int>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeliveryID">送货id</param>
        /// <param name="customerInfoID">客户id</param>
        void ExpressMath(int DeliveryID, int customerInfoID);
    }
}

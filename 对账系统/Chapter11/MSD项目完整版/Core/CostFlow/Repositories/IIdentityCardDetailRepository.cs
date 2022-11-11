using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CostFlow.Repositories
{
    /// <summary>
    /// 身份证验证流水接口
    /// </summary>
    public interface IIdentityCardDetailRepository : IDao<IdentityCardDetail, int>
    {
    }
}

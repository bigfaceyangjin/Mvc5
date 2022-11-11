using ProjectBase.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UserModule.Repositories
{
    public interface IUserAccountRepository : IDao<UserAccount, int>
    {
        UserAccount GetByAccount(string account);
    }
}

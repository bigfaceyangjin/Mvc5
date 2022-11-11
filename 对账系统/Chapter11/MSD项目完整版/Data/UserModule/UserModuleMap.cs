using Core.UserModule;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UserModule
{
    public class UserAccountMap : ClassMapping<UserAccount>
    {
        public UserAccountMap()
        {
            Table("UserAccount");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            Property(p => p.Account);
            Property(p => p.PassWord);
            Property(p => p.RealName);
            Property(p => p.CreateTime);
            Property(p => p.IsAvailable);
        }
    }
}

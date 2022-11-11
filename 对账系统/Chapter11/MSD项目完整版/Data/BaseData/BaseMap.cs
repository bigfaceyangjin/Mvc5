using Core.BaseData;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.BaseData
{
    /// <summary>
    /// 币种映射
    /// </summary>
    public class CurrencyMap : ClassMapping<Currency>
    {
        public CurrencyMap()
        {
            Table("Base_Currency");        
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            Property(p => p.FullName);
            Property(p => p.ShortName);
            Property(p => p.ExchangeRate);
            Property(p => p.Symbol);
            Property(p => p.CreateUser);
            Property(p => p.CreateTime);
            Property(p => p.VoidFlag);
            Property(p => p.IsDefault);      
        }
    }
    /// <summary>
    /// 费用项映射
    /// </summary>
    public class FeeItemMap : ClassMapping<FeeItem>
    {
        public FeeItemMap()
        {
            Table("Base_FeeItem");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            Property(p => p.FullName);
            Property(p => p.ShortName);
            Property(p => p.CreateUser);
            Property(p => p.CreateTime);
            Property(p => p.VoidFlag);       
        }
    }
}

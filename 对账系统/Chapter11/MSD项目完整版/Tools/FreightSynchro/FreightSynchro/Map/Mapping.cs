using FreightSynchro.Entitys;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightSynchro.Map
{
    public class WayBillInComeMap : ClassMapping<WayBillInCome>
    {
        public WayBillInComeMap()
        {
            Lazy(false);
            Table("WayBillInCome");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            ManyToOne(p => p.LoadBillBy, m => { m.Column("LoadBillID"); m.Cascade(Cascade.None); });
            Property(p => p.CustomerID, m => m.Update(false));
            Property(p => p.ExpressNo, m => m.Update(false));
            Property(p => p.Weight, m => m.Update(false));
            Property(p => p.ReceiverProvince, m => m.Update(false));
            Property(p => p.SenderProvince, m => m.Update(false));
            Property(p => p.ExpressFee);
            Property(p => p.OperateFee);
            Property(p => p.ExpressType, m => { m.Type<NHibernate.Type.EnumType<ExpressTypeEnum>>(); });
            Property(p => p.IsGetExpressFee);
            Property(p => p.PreExpressFee);
            Property(p => p.PreOperateFee);
            Bag(p => p.Items, m => { m.Key(k => k.Column("WayBillID")); m.Cascade(Cascade.All); }, rel => rel.OneToMany());
        }
    }

    public class WayBillFeeDetailMap : ClassMapping<WayBillFeeDetail>
    {
        public WayBillFeeDetailMap()
        {
            Lazy(false);
            Table("WayBillFeeDetail");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            ManyToOne(p => p.WayBillInComeBy, m => { m.Column("WayBillID"); m.Cascade(Cascade.None); });
            Property(p => p.FeeType, x => { x.Type<NHibernate.Type.EnumType<WaybillFeeType>>(); });
            Property(p => p.Fee);
            Property(p => p.FeeExplain);
            Property(p => p.CreateTime);
        }
    }

    public class ExpressNoExceptionDetailMap : ClassMapping<ExpressNoExceptionDetail>
    {
        public ExpressNoExceptionDetailMap()
        {
            Lazy(false);
            Table("ExpressNoExceptionDetail");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            Property(p => p.ExpressNo);
            Property(p => p.ExceptionType, x => { x.Type<NHibernate.Type.EnumType<ExceptionTypeEnum>>(); });
            Property(p => p.ExceptionMsg);
            Property(p => p.CreateTime);
            Property(p => p.LoadBillNO);
            Property(p => p.CustomerID);
        }
    }

    public class LoadBillInComeMap : ClassMapping<LoadBillInCome>
    {
        public LoadBillInComeMap()
        {
            Lazy(false);
            Table("LoadBillInCome");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            Property(p => p.LoadBillNum, m => m.Update(false));
            Property(p => p.TotalCollectFees);
            Property(p => p.TotalOperateFee);
        }
    }

    public class BaseFeeItemMap : ClassMapping<BaseFeeItem>
    {
        public BaseFeeItemMap()
        {
            Lazy(false);
            Table("Base_FeeItem");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            Property(p => p.FullName, m => m.Update(false));
            Property(p => p.Code, m => m.Update(false));
            Property(p => p.Fee, m => m.Update(false));
            Property(p => p.BusinessType, m => m.Update(false));
            Property(p => p.CreateUser, m => m.Update(false));
            Property(p => p.CreateTime, m => m.Update(false));
            Property(p => p.VoidFlag, m => m.Update(false));
        }
    }
}

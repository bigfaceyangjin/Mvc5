using Core;
using Core.CostFlow;
using Core.Reconciliation;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.CostFlow
{
   
    /// <summary>
    /// 身份证验证费用映射
    /// </summary>
    public class IdentityCardDetailMap : ClassMapping<IdentityCardDetail>
    {
        public IdentityCardDetailMap()
        {
            Table("IdentityCardDetail");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            //Property(p => p.DeliveryID);
            //ManyToOne(p => p.ExpressCurInfoBy, p => p.Column("DeliveryID"));          
            Property(p => p.Fee);
            Property(p => p.ExpressNo);
            Property(p => p.CreateTime);
            Property(p => p.ValideTime);
            Property(p => p.OrderID);
            Property(p => p.IdentityNmber);
            Property(p => p.IdentityName);
            Property(p => p.Status, m => { m.Type<NHibernate.Type.EnumType<Core.PayStatus>>(); m.Column("PayStatus"); });      
            //Property(p => p.CustomerID);
            ManyToOne(p => p.CustomerInfoBy, p => p.Column("CustomerID"));          
        }
    }
    /// <summary>
    /// 运单成本映射
    /// </summary>
    public class WayBillCostMap : ClassMapping<WayBillCost>
    {
        public WayBillCostMap()
        {
            Table("WayBillCost");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });    
            Property(p => p.ExpressNo);
            Property(p => p.BatchNO);
            Property(p => p.PostingTime);
            Property(p => p.Weight);
            Property(p => p.ProcessingFee);
            Property(p => p.WayBillFee);
            Property(p => p.SendAddress);
            Property(p => p.Product);

            //新加2015-06-30
            Property(p => p.ReconcileDate);
            Property(p => p.PayStatus);
            Property(p => p.PayTime);
        }
    }

    public class LoadBillCostMap : ClassMapping<LoadBillCost>
    {
        public LoadBillCostMap()
        {
            Table("LoadBillCost");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });    
            Property(p => p.LoadBillNum);
            Property(p => p.LoadTime);
            Property(p => p.GroundHandlingFee);
            Property(p => p.StoreFee);
            Property(p => p.CreateTime);
            Property(p => p.DeliveryTime);
            Property(p => p.FactWeight);
            Property(p => p.FeeWeight);
            Property(p => p.TotalFee);
            Property(p => p.StorageFeeReason);
            Property(p => p.ReconcileDate);
            Property(p => p.PayStatus, m => { m.Type<NHibernate.Type.EnumType<PayStatus>>(); });
            Property(p => p.PayTime);
            Property(p => p.Status, m => { m.Type<NHibernate.Type.EnumType<LoadBillExceptionTypeEnum>>();});

            Property(p => p.CustomerID);
           
        }
    }
}

using Core;
using Core.ExceptionDir;
using Core.Reconciliation;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
/* ==============================================================================
   * 功能描述：ExceptionMap  
   * 创 建 者：Zouqj
   * 创建日期：2015/6/26 11:02:54
   ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ExceptionDir
{
   public class DelLoadBillCostExMap : ClassMapping<DelLoadBillCostEx>
   {
       public DelLoadBillCostExMap()
       {
           Table("DelLoadBillCostEx");
           Id(p => p.ID, m => { m.Generator(Generators.Identity);});
           Property(p=>p.LoadBillNum);
           Property(p=>p.LoadTime);
           Property(p=>p.GroundHandlingFee);
           Property(p=>p.StoreFee);
           Property(p=>p.CreateTime);
           Property(p=>p.DeliveryTime);
           Property(p=>p.FactWeight);
           Property(p=>p.FeeWeight);
           Property(p=>p.TotalFee);
           Property(p=>p.StorageFeeReason);
           Property(p=>p.ReconcileDate);
           Property(p => p.PayStatus, m => m.Type<NHibernate.Type.EnumType<PayStatus>>());
           Property(p=>p.PayTime);
           Property(p => p.Status, m => m.Type<NHibernate.Type.EnumType<LoadBillExceptionTypeEnum>>());
           Property(p => p.DelTime);

           Property(p => p.CustomerID);
           Property(p => p.TotalOtherFee);
       }
   }

   public class DelWayBillCostExMap : ClassMapping<DelWayBillCostEx>
   {
       public DelWayBillCostExMap()
       {
            Table("DelWayBillCostEx");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); });
            Property(p => p.ExpressNo);
            Property(p => p.WayBillFee);
            Property(p => p.Weight);
            Property(p => p.CreateTime);
            Property(p => p.SendAddress);
            Property(p => p.ProcessingFee);
            Property(p => p.Product);
            Property(p => p.BatchNO);
            Property(p => p.PostingTime);
            Property(p => p.ReconcileDate);
            Property(p => p.DelTime);
       }
   }

   public class DelExpressNoExceptionDetailMap : ClassMapping<DelExpressNoExceptionDetail>
   {
       public DelExpressNoExceptionDetailMap()
       {
            Table("DelExpressNoExceptionDetail");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); });
            Property(p => p.ExpressNo);
            Property(p => p.ExceptionType, m => m.Type<NHibernate.Type.EnumType<WayBillExceptionTypeEnum>>());
            Property(p => p.ExceptionMsg);
            Property(p => p.CreateTime);
            Property(p => p.LoadBillNO);
            ManyToOne(p => p.DelWayBillCostExBy, m => { m.Column("WayBillCostID"); m.Cascade(Cascade.All); m.Unique(true); });
            Property(p => p.PostingTime);
            Property(p => p.DelTime);
       }
   }

   /// <summary>
   /// 运单异常数据
   /// </summary>
   public class WayBillExceptionMap : ClassMapping<WayBillException>
   {   
       public WayBillExceptionMap()
       {
           Table("ExpressNoExceptionDetail");
           Id(p => p.ID, m => { m.Generator(Generators.Identity); });
           Property(p => p.ExpressNo);
           Property(p => p.ExceptionType);
           Property(p => p.ExceptionMsg);
           Property(p => p.LoadBillNum, m => { m.Column("LoadBillNO"); });   //
           Property(p => p.WayBillCostID);
           Property(p => p.PostingTime);
       }
   }
}

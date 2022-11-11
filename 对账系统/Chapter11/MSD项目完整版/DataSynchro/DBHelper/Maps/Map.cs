using DBHelper.Entitys;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper.Maps
{
    public class B2CCustomerMap : ClassMapping<ExpressCurInfo>
    {
        public B2CCustomerMap()
        {
            Table("ExpressCurInfo");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            Property(p => p.BusinessID,m=>{m.Column("DeliveryID");});
            Property(p => p.BusinessName,m=>{m.Column("DeliveryName");});
            Property(p => p.UserName,m=>{m.Column("AccountName");});
            Property(p => p.CreateTime);
        }
    }

    public class LoadBillMap : ClassMapping<LoadBillInfo>
    {
        public LoadBillMap()
        {
            Table("LoadBillInCome");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            Property(p => p.BusinessId, m => { m.Column("DeliveryID"); m.Update(false); });
            Property(p=>p.CompanyId,m=>m.Update(false));
            Property(p => p.CustomsTime, m => { m.Column("CompletionTime"); m.Update(false); });
            Property(p => p.LoadBillId, m => { m.Column("BusinessID"); m.Update(false); });
            Property(p => p.LoadBillNo, m => { m.Column("LoadBillNum"); m.Update(false); });
            Property(p => p.Status, m => { m.Column("BillStatus"); m.Update(false); });
            Property(p => p.Weight, m =>{m.Column("BillWeight");m.Update(false);});
            Property(p=>p.CustomerID,m=>m.Update(false));
            Property(p=>p.OrderCounts);
            Property(p => p.LoadFee, m => m.Update(false));
            Property(p => p.StoreFee, m => m.Update(false));
            Property(p => p.TotalCollectFees, m => m.Update(false));
            Property(p => p.OtherFee, m => m.Update(false));
            Property(p => p.TotalReceivableFee, m => m.Update(false));
            Property(p => p.CreateTime,m=>m.Update(false));
            Property(p => p.IsGet);
        }
    }

    public class WayBillInfoMap : ClassMapping<WayBillInfo>
    {
        public WayBillInfoMap()
        {
            Table("WayBillInCome");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            Property(p => p.Address, m => m.Column("ReceiverAddress"));
            Property(p => p.BusinessId, m => m.Column("DeliveryID"));
            Property(p => p.CompanyId);
            Property(p => p.CreateTime);
            Property(p => p.ExpressNo);
            Property(p => p.OrderId, m => m.Column("BusinessID"));
            Property(p => p.Province, m => m.Column("ReceiverProvince"));
            Property(p => p.Receiver, m => m.Column("ReceiverName"));
            Property(p => p.Weight);
            Property(p => p.LoadBillID);
            Property(p => p.LoadBillNO);
            Property(p => p.OrderCode, m => m.Column("OrderNo"));
            Property(p => p.SenderProvince);
            Property(p => p.CustomerID);
            Property(p => p.ExpressFee);
        }
    }

    public class LoadBillInComeDetailMap : ClassMapping<LoadBillInComeDetail>
    {
        public LoadBillInComeDetailMap()
        {
            Table("LoadBillInComeDetail");
            Id(p => p.ID, m => { m.Generator(Generators.Identity); m.Column("ID"); });
            Property(p => p.CreateTime);
            Property(p => p.DetailId, m => m.Column("BussinessID"));
            Property(p => p.FeeExplain);
            Property(p => p.Fee);
            Property(p => p.FeeType);
            Property(p => p.BillID);
        }
    }


    /// <summary>
    /// 同步仓租实体
    /// </summary>
    public class BillWarehouseModelMap : ClassMapping<BillWarehouseModel>
    {
        public BillWarehouseModelMap()
        {
            Table("LoadBillInCome");
            Id(p => p.ID, m => { m.Generator(Generators.Identity);});
            Property(p => p.CustomsTime, m => m.Column("CompletionTime"));
            Property(p => p.Fee, m => m.Column("StoreFee"));
            Property(p => p.Status, m => m.Column("BillStatus"));
            Property(p => p.LoadTime);
            Property(p => p.DeliveryTime);
            Property(p => p.StorageFeeReason);
        }
    }
}
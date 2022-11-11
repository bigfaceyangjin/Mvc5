using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightSynchro.Entitys
{
    public class WayBillFeeDetail : BaseEntity<WayBillFeeDetail, int>
    {
        /// <summary>
        /// 运单号ID
        /// </summary>
        public virtual WayBillInCome WayBillInComeBy { get; set; }
        /// <summary>
        /// 费用类型
        /// </summary>
        public virtual WaybillFeeType FeeType { get; set; }
        /// <summary>
        /// 费用
        /// </summary>
        public virtual decimal Fee { get; set; }
        /// <summary>
        /// 费用简介
        /// </summary>
        public virtual string FeeExplain { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        public static WayBillFeeDetail Save(WayBillInCome w, WaybillFeeType feeType, string feeExplain)
        {
            WayBillFeeDetail operateDetail = new WayBillFeeDetail();
            operateDetail.WayBillInComeBy = w;
            operateDetail.FeeType = feeType;
            operateDetail.Fee = w.OperateFee;
            operateDetail.CreateTime = DateTime.Now;
            operateDetail.FeeExplain = feeExplain;
            return operateDetail;
        }

        public static void SaveTesting(WayBillInCome w, WaybillFeeType feeType, string feeExplain)
        {
            WayBillFeeDetail operateDetail = new WayBillFeeDetail();
            operateDetail.WayBillInComeBy = w;
            operateDetail.FeeType = feeType;
            operateDetail.Fee = w.OperateFee;
            operateDetail.CreateTime = DateTime.Now;
            operateDetail.FeeExplain = feeExplain;
        }
    }


    /// <summary>
    /// 费用类型
    /// </summary>
    public enum WaybillFeeType
    {
        /// <summary>
        /// 基本运费
        /// </summary>
        [Description("基本运费")]
        ExpressFee = 1,
        /// <summary>
        /// 提货费
        /// </summary>
        [Description("提货费")]
        DeliveryFee = 2,
        /// <summary>
        ///	危险标识处理费 3
        /// </summary>
        [Description("危险标识处理费")]
        DangerMarkFee = 3,
        /// <summary>
        ///	换单费 4
        /// </summary>
        [Description("换单费")]
        ChangeOrderFee = 4,
        /// <summary>
        ///	陆运费(自提) 5
        /// </summary>
        [Description("陆运费(自提)")]
        LuShippingFee = 5,
        /// <summary>
        ///	陆运费(二次派送) 6
        /// </summary>
        [Description("陆运费(二次派送)")]
        LuShippingTwoFee = 6,
        /// <summary>
        ///	身份证验证费 7
        /// </summary>
        [Description("身份证验证费")]
        IdentityVerifyFee = 7,
        /// <summary>
        ///	标准快递清关费 8
        /// </summary>
        [Description("标准快递清关费")]
        StandardClearanceFee = 8,
        /// <summary>
        ///	经济快递清关费 9
        /// </summary>
        [Description("经济快递清关费")]
        EconomyClearanceFee = 9,
        /// <summary>
        ///	重新申报操作费 10
        /// </summary>
        [Description("重新申报操作费")]
        RepeatDeclarationFee = 10,
        /// <summary>
        ///	退件再派送费 11
        /// </summary>
        [Description("退件再派送费")]
        BackPiecesDeliveryFee = 11,
        /// <summary>
        ///	有信息无件费 12
        /// </summary>
        [Description("有信息无件费")]
        ThereInfoNoMemberFee = 12,
        /// <summary>
        ///	丢失件 13
        /// </summary>
        [Description("丢失件")]
        LoseFee = 13,
        /// <summary>
        ///	外包装破损件 14
        /// </summary>
        [Description("外包装破损件")]
        DamagedPackaging = 14
    }
}

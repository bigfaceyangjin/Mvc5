using Core.CostFlow;
using Core.Customer.Repositories;
using Core.Filters;
using Core.Reconciliation.Repositories;
using ProjectBase.Data;
using ProjectBase.Utils.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Reconciliation
{
    /// <summary>
    /// 异常提单数据
    /// </summary>
    public class UnusualLoadBill : DomainObject<UnusualLoadBill, int, IUnusualLoadBillRepository>
    {
        #region property
        /// <summary>
        /// 客户简称
        /// </summary>
        public virtual string CusName { get; set; }
        /// <summary>
        /// 提单号
        /// </summary>
        public virtual string LoadBillNum { get; set; }
        /// <summary>
        /// 计费重量（kg）
        /// </summary>
        public virtual decimal FeeWeight { get; set; }
        /// <summary>
        /// 包裹数量
        /// </summary>
        public virtual Int64 ExpressCount { get; set; }
        /// <summary>
        /// 包裹重量
        /// </summary>
        public virtual decimal ExpressWeight { get; set; }
        /// <summary>
        /// 清关完成时间
        /// </summary>
        public virtual DateTime CompletionTime { get; set; }
        /// <summary>
        /// 邮政地勤费(邮政提货费)
        /// </summary>
        public virtual decimal GroundHandlingFee { get; set; }
        /// <summary>
        /// 邮政仓租
        /// </summary>
        public virtual decimal CostStoreFee { get; set; }
        /// <summary>
        /// 邮政邮资
        /// </summary>
        public virtual decimal CostExpressFee { get; set; }
        /// <summary>
        /// 邮件处理费
        /// </summary>
        public virtual decimal CostOperateFee { get; set; }
        /// <summary>
        /// 其他费用
        /// </summary>
        public virtual decimal CostOtherFee { get; set; }
        /// <summary>
        /// 总成本
        /// </summary>
        public virtual decimal CostTotalFee { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual PayStatus CostStatus { get; set; }
        /// <summary>
        /// 客户提货费
        /// </summary>
        public virtual decimal InComeLoadFee { get; set; }
        /// <summary>
        /// 客户仓租
        /// </summary>
        public virtual decimal InComeStoreFee { get; set; }
        /// <summary>
        /// 客户运费
        /// </summary>
        public virtual decimal InComeExpressFee { get; set; }
        /// <summary>
        /// 客户操作费
        /// </summary>
        public virtual decimal InComeOperateFee { get; set; }
        /// <summary>
        /// 其他费用
        /// </summary>
        public virtual decimal InComeOtherFee { get; set; }
        /// <summary>
        /// 总收入
        /// </summary>
        public virtual decimal InComeTotalFee { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual PayStatus InComeStatus { get; set; }
        /// <summary>
        /// 总毛利
        /// </summary>
        public virtual decimal TotalGrossProfit { get; set; }
        /// <summary>
        /// 毛利率
        /// </summary>
        public virtual decimal GrossProfitRate { get; set; }
        /// <summary>
        /// 提单异常状态
        /// </summary>
        public virtual LoadBillExceptionTypeEnum Status { get; set; }
        /// <summary>
        /// 对账日期
        /// </summary>
        public virtual DateTime ReconcileDate { get; set; }
        #endregion

        #region common method
        public static long GetUnusualLoadBillCount(ParameterFilter filter)
        {
            return Dao.GetUnusualLoadBillCount(filter);
        }

        public static IPageOfList<UnusualLoadBill> GetUnusualByFilter(ParameterFilter filter)
        {
            return Dao.GetUnusualByFilter(filter);
        }
        public static List<long> GetUnusualLoadBillCount()
        {
            return Dao.GetUnusualLoadBillCount();
        }
        #endregion
    }

    public class LoadBillCost : DomainObject<LoadBillCost, int, ILoadBillCostRepository>
    {
        #region property
        /// <summary>
        /// 提单号
        /// </summary>
        public virtual string LoadBillNum { get; set; }
        /// <summary>
        /// 提货日期
        /// </summary>
        public virtual DateTime LoadTime { get; set; }
        /// <summary>
        /// 地勤费
        /// </summary>
        public virtual decimal GroundHandlingFee { get; set; }
        /// <summary>
        /// 仓租
        /// </summary>
        public virtual decimal StoreFee { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 到货时间
        /// </summary>
        public virtual DateTime DeliveryTime { get; set; }
        /// <summary>
        /// 实重
        /// </summary>
        public virtual decimal FactWeight { get; set; }
        /// <summary>
        /// 计费重量(kg)
        /// </summary>
        public virtual decimal FeeWeight { get; set; }
        /// <summary>
        /// 总成本
        /// </summary>
        public virtual decimal TotalFee { get; set; }
        /// <summary>
        /// 产生仓租原因
        /// </summary>
        public virtual string StorageFeeReason { get; set; }
        /// <summary>
        /// 结算月份
        /// </summary>
        public virtual DateTime ReconcileDate { get; set; }
        /// <summary>
        /// 结算状态
        /// </summary>
        public virtual Core.PayStatus PayStatus { get; set; }
        /// <summary>
        /// 结算时间
        /// </summary>
        public virtual DateTime PayTime { get; set; }
        /// <summary>
        /// 异常状态
        /// </summary>
        public virtual LoadBillExceptionTypeEnum Status { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public virtual int CustomerID { get; set; }

        /// <summary>
        /// 其他费用
        /// </summary>
        //public virtual decimal TotalOtherFee { get; set; }
        #endregion

        #region common method
        public static IList<LoadBillCost> GetByLoadBillNum(string loadBillNum)
        {
            return Dao.GetByLoadBillNum(loadBillNum);
        }
        #endregion

        #region entity method
        public virtual bool SetNormal(out string message)
        {
            if (Status==LoadBillExceptionTypeEnum.ImportRepeat)
            {
                if (GetByLoadBillNum(LoadBillNum).Where(a=>a.Status==LoadBillExceptionTypeEnum.Normal).Count()>0)
                {
                    message = string.Format("{0}存在正常成本数据,不能进行设置正常操作", LoadBillNum, Status.ToChinese());
                    return false;
                }
                Status = LoadBillExceptionTypeEnum.Normal;
                this.Update();
                message = "操作成功";
                return true;
            }
            else
	        {
                message = string.Format("{0}属于{1},不能进行设置正常操作", LoadBillNum, Status.ToChinese());
                return false;
	        }
        }

        public virtual long GetWayBillCount()
        {
            return WayBillCost.GetCountByLoadBillNum(LoadBillNum);
        }

        public virtual bool DeleteRecord()
        {
            using (var tran=Dao.BeginTransaction())
            {
                try
                {
                    //删除成本表
                    this.Delete();
                    new Core.ExceptionDir.DelLoadBillCostEx()
                    {
                        LoadBillNum = this.LoadBillNum,//航空提单号
                        LoadTime = this.LoadTime,//提货日期
                        GroundHandlingFee = this.GroundHandlingFee,//地勤费
                        StoreFee = this.StoreFee, //仓租
                        CreateTime = DateTime.Now,//创建时间
                        DeliveryTime = this.DeliveryTime,//到货时间
                        FactWeight = this.FactWeight,//实重
                        FeeWeight = this.FeeWeight,//计费重量(kg)
                        TotalFee = this.TotalFee,//总成本
                        StorageFeeReason = this.StorageFeeReason,//产生仓租原因
                        CustomerID = this.CustomerID,//客户ID
                        ReconcileDate = this.ReconcileDate,//结算月份
                        PayStatus = this.PayStatus,//结算状态
                        PayTime = this.PayTime,//结算时间
                        TotalOtherFee = 0,//其他费用 成本表没有其他费用
                        Status = this.Status,//异常状态
                        DelTime = DateTime.Now//删除时间
                    }.Save();
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        #endregion
    }
}

using NHibernate;
using NHibernate.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FreightSynchro.Entitys
{
    public class WayBillInCome : BaseEntity<WayBillInCome, int>
    {
        #region property
        /// <summary>
        /// 提货单号ID
        /// </summary>
        public virtual LoadBillInCome LoadBillBy { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public virtual int CustomerID { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public virtual string ExpressNo { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public virtual decimal Weight { get; set; }
        /// <summary>
        /// 收件人省份
        /// </summary>
        public virtual string ReceiverProvince { get; set; }
        /// <summary>
        /// 发件人省份
        /// </summary>
        public virtual string SenderProvince { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public virtual decimal ExpressFee { get; set; }
        /// <summary>
        /// 操作费
        /// </summary>
        public virtual decimal OperateFee { get; set; }
        /// <summary>
        /// 预估运费
        /// </summary>
        public virtual decimal PreExpressFee { get; set; }
        /// <summary>
        /// 预估操作费
        /// </summary>
        public virtual decimal PreOperateFee { get; set; }
        /// <summary>
        /// 快递类型
        /// </summary>
        public virtual ExpressTypeEnum ExpressType { get; set; }
        /// <summary>
        /// 是否已计算运费
        /// 0：未计算
        /// 1：已计算
        /// </summary>
        public virtual int IsGetExpressFee { get; set; }
        /// <summary>
        /// 对应对账数据
        /// </summary>
        public virtual IList<WayBillFeeDetail> Items { get; set; }
        #endregion

        /// <summary>
        /// 根据收货商ID获取收货商数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static IList<WayBillInCome> GetByNotExpressFee(int pageSize)
        {
            var query = NHibernateSession.CreateQuery("FROM WayBillInCome WHERE IsGetExpressFee=0");
            return query.SetMaxResults(pageSize).List<WayBillInCome>();
        }

        public virtual bool GetFee(string url)
        {
            using (var trans = NHibernateSession.BeginTransaction())
            {
                try
                {
                    SinglePostMode mode = new SinglePostMode(this);
                    string msg = string.Empty;
                    // 获取运费
                    Dictionary<string, decimal> fee = GetFreight(url, mode);
                    LogHelper.WriteLog(msg);
                    if (fee["ExpressFee"] > 0)
                    {
                        List<WayBillFeeDetail> li = new List<WayBillFeeDetail>();
                        // 更新运费，预估运费
                        this.ExpressFee = fee["ExpressFee"];
                        this.PreExpressFee = fee["PreExpressFee"];
                        // 已计算运费
                        this.IsGetExpressFee = 1;
                        // 添加操作费流水
                        li.Add(WayBillFeeDetail.Save(this, mode.feeType, mode.feeExplain));
                        // 添加运费流水
                        li.Add(WayBillFeeDetail.Save(this, WaybillFeeType.ExpressFee, "运费流水。" + mode.ToString()));
                        Items = li;
                        this.Merge();
                        msg = string.Format("运单【{0}】完成运费更新。", this.ExpressNo);
                    }
                    else
                    {
                        // 已计算运费
                        this.IsGetExpressFee = 1;
                        this.Merge();
                        msg = string.Format("运单【{0}】的运费为0，更新失败。\r\n{1}", ExpressNo, mode.ToString());
                        LogHelper.WriteLog(msg);
                    }
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    string msg = string.Format("运单【{0}】更新运费失败。", this.ExpressNo);
                    LogHelper.ErrorLog(msg, ex);
                    return false;
                }
            }
        }

        public virtual void SetFreight(Freight f)
        {
            using (var trans = NHibernateSession.BeginTransaction())
            {
                try
                {
                    string msg = string.Empty;
                    foreach (FreightItem item in f.List)
                    {
                        // 更新运费，预估运费
                        if (item.ShipCode.Equals("TenThousand") || item.ShipCode.Equals("EMS"))//收入运费
                            this.ExpressFee = item.Freight;
                        else if (item.ShipCode.Equals("ExpectedFreight"))//预估成本运费
                            this.PreExpressFee = item.Freight;
                    }
                    if (this.ExpressFee > 0)
                    {
                        WaybillFeeType feeType;
                        List<WayBillFeeDetail> li = new List<WayBillFeeDetail>();
                        string feeExplain = string.Empty;
                        if (this.ExpressType == ExpressTypeEnum.Economy)
                        {
                            // 经济快递
                            feeType = WaybillFeeType.EconomyClearanceFee;
                            feeExplain = "经济快递操作费流水";
                        }
                        else
                        {
                            // 标准快递
                            feeType = WaybillFeeType.StandardClearanceFee;
                            feeExplain = "标准快递操作费流水";
                        }
                        // 已计算运费
                        this.IsGetExpressFee = 1;

                        // 添加操作费流水
                        li.Add(WayBillFeeDetail.Save(this, feeType, feeExplain));
                        // 添加运费流水
                        li.Add(WayBillFeeDetail.Save(this, WaybillFeeType.ExpressFee, "运费流水。"));
                        Items = li;
                        this.Merge();
                        msg = string.Format("运单【{0}】完成运费更新。", this.ExpressNo);
                    }
                    else
                    {
                        // 已计算运费
                        this.IsGetExpressFee = 1;
                        this.Merge();
                        msg = string.Format("运单【{0}】的运费为0，更新失败。", ExpressNo);
                        LogHelper.WriteLog(msg);
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    string msg = string.Format("运单【{0}】更新运费失败。", this.ExpressNo);
                    LogHelper.ErrorLog(msg, ex);
                }
            }
        }

        public virtual WayBillInCome GetWayBillInCome(Freight f)
        {
            string msg = string.Empty;
            foreach (FreightItem item in f.List)
            {
                // 更新运费，预估运费
                if (item.ShipCode.Equals("TenThousand") || item.ShipCode.Equals("EMS"))//收入运费
                    this.ExpressFee = item.Freight;
                else if (item.ShipCode.Equals("ExpectedFreight"))//预估成本运费
                    this.PreExpressFee = item.Freight;
            }
            if (this.ExpressFee > 0)
            {
                WaybillFeeType feeType;
                List<WayBillFeeDetail> li = new List<WayBillFeeDetail>();
                string feeExplain = string.Empty;
                if (this.ExpressType == ExpressTypeEnum.Economy)
                {
                    // 经济快递
                    feeType = WaybillFeeType.EconomyClearanceFee;
                    feeExplain = "经济快递操作费流水";
                }
                else
                {
                    // 标准快递
                    feeType = WaybillFeeType.StandardClearanceFee;
                    feeExplain = "标准快递操作费流水";
                }
                // 已计算运费
                this.IsGetExpressFee = 1;
                // 添加操作费流水
                li.Add(WayBillFeeDetail.Save(this, feeType, feeExplain));
                // 添加运费流水
                li.Add(WayBillFeeDetail.Save(this, WaybillFeeType.ExpressFee, "运费流水。"));
                Items = li;
            }
            else
            {
                // 已计算运费
                this.IsGetExpressFee = 1;
            }
            return this;
        }

        public static void BatchUpdate(IList<WayBillInCome> li)
        {
            string msg = string.Empty;
            string ExpressNo = string.Empty;
            using (IStatelessSession s = NHibernateSession.SessionFactory.OpenStatelessSession())
            {
                //NHibernateSession.SessionFactory.Evict(typeof(WayBillInCome));
                var tran = s.BeginTransaction();
                try
                {
                    foreach (var item in li)
                    {
                        ExpressNo = item.ExpressNo;
                        if (item.ExpressFee > 0)
                        {
                            msg = string.Format("运单【{0}】完成运费更新。", item.ExpressNo);
                        }
                        else
                        {
                            msg = string.Format("运单【{0}】的运费为0，更新失败。", ExpressNo);
                            LogHelper.WriteLog(msg);
                        }
                        s.Update(item);
                        if (item.Items.Count > 0)
                        {
                            foreach (var iitem in item.Items)
                            {
                                s.Insert(iitem);
                            }
                        }
                    }
                    tran.Commit();
                    NHibernateSession.Clear();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    msg = string.Format("运单【{0}】更新运费失败。", ExpressNo);
                    LogHelper.ErrorLog(msg, ex);
                }
                finally
                {
                    s.Dispose();
                }
            }
        }

        public virtual void SetExpressType(string express)
        {
            if (!string.IsNullOrEmpty(express) && !string.IsNullOrEmpty(this.ReceiverProvince))
            {
                string pr = this.ReceiverProvince;
                if (this.ReceiverProvince.Length > 2)
                    pr = this.ReceiverProvince.Substring(0, 2);
                if (express.Contains(pr))//经济
                    this.ExpressType = ExpressTypeEnum.Economy;
                else
                    this.ExpressType = ExpressTypeEnum.Standard;
            }
        }

        protected virtual Dictionary<string, decimal> GetFreight(string url, SinglePostMode mode)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            result.Add("ExpressFee", 0);//收入运费
            result.Add("PreExpressFee", 0);//预估运费
            try
            {
                string rsString = WebApiClient<SinglePostMode>.Post(url, mode);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rsString);
                XmlNamespaceManager nameSpace = new XmlNamespaceManager(xmlDoc.NameTable);
                nameSpace.AddNamespace("abc", "http://schemas.datacontract.org/2004/07/Uuch.Ucw.Core.ShipPrice");
                XmlNodeList xmlNode = xmlDoc.SelectNodes("abc:ArrayOfShipPriceResult/abc:ShipPriceResult", nameSpace);
                foreach (XmlNode item in xmlNode)
                {
                    decimal value = 0;
                    string key = item.SelectSingleNode("abc:ShipCode", nameSpace).InnerText;
                    decimal.TryParse(item.SelectSingleNode("abc:Total", nameSpace).InnerText, out value);
                    if (key == "TenThousand" || key == "EMS")//收入运费
                    {
                        result["ExpressFee"] = value;
                    }
                    else if (key == "ExpectedFreight")//预估成本运费
                    {
                        result["PreExpressFee"] = value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static WayBillInCome GetWayBillInComeByNo(string expressNo)
        {
            var query = NHibernateSession.CreateQuery("FROM WayBillInCome WHERE ExpressNo = :ExpressNo");
            query.SetParameter("ExpressNo", expressNo);
            return query.List<WayBillInCome>().FirstOrDefault();
        }

        public static string GetFeeTesting(string url, string expressNo, string express)
        {
            StringBuilder result = new StringBuilder();
            string msg = string.Empty;
            try
            {
                WayBillInCome wayBill = WayBillInCome.GetWayBillInComeByNo(expressNo);
                wayBill.SetExpressType(express);
                SinglePostMode mode = new SinglePostMode(wayBill);
                // 获取运费
                Dictionary<string, decimal> fee = wayBill.GetFreight(url, mode);
                if (fee["ExpressFee"] > 0)
                {
                    // 更新运费，预估运费
                    wayBill.ExpressFee = fee["ExpressFee"];
                    wayBill.PreExpressFee = fee["PreExpressFee"];
                    // 已计算运费
                    wayBill.IsGetExpressFee = 1;
                }
                result.Append("运单号:").AppendLine(wayBill.ExpressNo);
                result.Append("运费:").AppendLine(wayBill.ExpressFee.ToString());
                result.Append("预估运费:").AppendLine(wayBill.PreExpressFee.ToString());
                result.Append("快递类型：").AppendLine(wayBill.ExpressType == ExpressTypeEnum.Economy ? "经济快递" : "标准快递");
                result.AppendLine();
                result.AppendLine();
                result.Append("系统:").AppendLine(mode.System);
                result.Append("运费方式:").AppendLine(mode.ClientLevel);
                result.Append("寄件地:").AppendLine(mode.From);
                result.Append("目的地:").AppendLine(mode.To);
                result.Append("重量:").AppendLine(mode.Weight.ToString());

            }
            catch (Exception ex)
            {
                msg = string.Format("运单【{0}】计算运费失败。", expressNo);
            }
            if (!string.IsNullOrEmpty(msg))
            {
                result.AppendLine();
                result.AppendLine();
                result.AppendFormat("<Message>{0}</Message>", msg);
            }
            return result.ToString();
        }
    }

    public class SinglePostMode
    {
        //系统：Finance
        //寄件地：广州
        //重量：2.15 （单位：千克)
        //目的地：广东（收件人省份前2位）
        //快递类型：B2CStage(普通)、B2CBoon(经济)
        //运费方式：EMS(经济快递)、TenThousand(一万件一下普通快递)、ThirtyThousand(一万至三万)、More-ThirtyThousand(三万以上)

        /// <summary>
        /// 系统
        /// </summary>
        public string System { get; set; }
        /// <summary>
        /// 运费方式
        /// </summary>
        public string ClientLevel { get; set; }
        /// <summary>
        /// 寄件地
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 目的地
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }
        ///// <summary>
        ///// 快递类型
        ///// </summary>
        //public string Ship { get; set; }

        internal WaybillFeeType feeType;

        internal string feeExplain;

        public SinglePostMode()
        { }
        public SinglePostMode(WayBillInCome w)
        {
            this.System = "Finance";
            if (w.SenderProvince.Length > 2)
                this.From = string.IsNullOrEmpty(w.SenderProvince) ? "" : w.SenderProvince.Substring(0, 2);
            else
                this.From = w.SenderProvince;
            if (w.ReceiverProvince.Length > 2)
                this.To = string.IsNullOrEmpty(w.ReceiverProvince) ? "" : w.ReceiverProvince.Substring(0, 2);
            else
                this.To = w.ReceiverProvince;
            this.Weight = w.Weight;
            string feeExplain = string.Empty;
            if (w.ExpressType == ExpressTypeEnum.Economy)
            {
                // 经济快递 10
                //this.Ship = "EMS";
                this.ClientLevel = "B2CBoon";
                feeType = WaybillFeeType.EconomyClearanceFee;
                feeExplain = "经济快递操作费流水";
            }
            else
            {
                // 标准快递 8
                //this.Ship = "TenThousand";
                this.ClientLevel = "B2CStage";
                feeType = WaybillFeeType.StandardClearanceFee;
                feeExplain = "标准快递操作费流水";
            }
        }
    }

    public class FreightModel
    {
        /// <summary>
        /// 运单号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 走货渠道
        /// </summary>
        public string ClientLevel { get; set; }
        /// <summary>
        /// 发寄城市
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 收件省份
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }

        public FreightModel() { }
        public FreightModel(WayBillInCome w)
        {
            this.Code = w.ExpressNo;
            if (w.SenderProvince.Length > 2)
                this.From = string.IsNullOrEmpty(w.SenderProvince) ? "" : w.SenderProvince.Substring(0, 2);
            else
                this.From = w.SenderProvince;
            if (w.ReceiverProvince.Length > 2)
                this.To = string.IsNullOrEmpty(w.ReceiverProvince) ? "" : w.ReceiverProvince.Substring(0, 2);
            else
                this.To = w.ReceiverProvince;
            this.Weight = w.Weight;
            if (w.ExpressType == ExpressTypeEnum.Economy)
            {
                // 经济快递
                this.ClientLevel = "B2CBoon";
            }
            else
            {
                // 标准快递
                this.ClientLevel = "B2CStage";
            }
        }

        //public WaybillFeeType GetFeeType()
        //{
        //    return feeType;
        //}

        //public string GetFeeExplain()
        //{
        //    return feeExplain;
        //}
    }

    /// <summary>
    /// 快递类型
    /// </summary>
    public enum ExpressTypeEnum
    {
        /// <summary>
        ///	标准快递
        /// </summary>
        [Description("标准快递")]
        Standard = 0,
        /// <summary>
        ///	经济快递
        /// </summary>
        [Description("经济快递")]
        Economy = 1,
    }
    /// <summary>
    /// 数据异常类型
    /// </summary>
    public enum ExceptionTypeEnum
    {
        /// <summary>
        /// 运费异常
        /// </summary>
        [Description("运费异常")]
        WayBillFeeException = 0,
    }
}

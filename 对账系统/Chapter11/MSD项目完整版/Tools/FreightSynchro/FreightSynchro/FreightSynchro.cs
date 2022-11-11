using FreightSynchro.Entitys;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FreightSynchro
{
    public delegate void MessageHandler(string message);
    public class FreightSynchro
    {
        public event MessageHandler Message;
        private int pageSize = 10000;
        /// <summary>
        /// 开始同步运费
        /// </summary>
        public void Start()
        {
            string economy = ConfigurationManager.AppSettings["Economy"];
            bool isBatchCalculate = ConfigurationManager.AppSettings["IsBatchCalculate"].Equals("Y") ? true : false;
            string url = string.Empty;
            if (isBatchCalculate)
            {
                url = ConfigurationManager.AppSettings["BatchFreightAPI"];
                pageSize = 500;
            }
            else
            {
                url = ConfigurationManager.AppSettings["SingleFreightAPI"];
                pageSize = 10000;
            }

            Dictionary<string, decimal> baseFee = BaseFeeItem.GetBySE();
            IList<WayBillInCome> list = null;
            List<int> loadBillList = new List<int>();
            do
            {
                list = WayBillInCome.GetByNotExpressFee(pageSize);
                list.ToList().ForEach(w => { w.SetExpressType(economy); });
                if (!isBatchCalculate)//单个计算
                {
                    foreach (WayBillInCome w in list)
                    {
                        if (w.ExpressType == ExpressTypeEnum.Economy)//经济
                        {
                            w.OperateFee = baseFee["E_OpratorFee"];
                            w.PreOperateFee = baseFee["PreE_OpratorFee"];
                        }
                        else
                        {
                            w.OperateFee = baseFee["S_OpratorFee"];
                            w.PreOperateFee = baseFee["PreS_OpratorFee"];
                        }
                        if (w.GetFee(url) && !loadBillList.Contains(w.LoadBillBy.ID))
                        {
                            loadBillList.Add(w.LoadBillBy.ID);
                        }
                        if (Message != null)
                        {
                            Message(string.Format("{0}计算运费", w.ExpressNo));
                        }
                    }
                }
                else//批量计算
                {
                    if (loadBillList == null)
                        loadBillList = new List<int>();
                    loadBillList.AddRange(this.BatchCalculate(url, list, economy, baseFee));
                }
                if (loadBillList.Count > 0)
                {
                    LoadBillInCome.UpdateFee(loadBillList);
                    loadBillList.Clear();
                }
            }
            while (list.Count > 0);
        }

        private List<int> BatchCalculate(string url, IList<WayBillInCome> list, string economy, Dictionary<string, decimal> baseFee)
        {
            List<int> result = new List<int>();
            List<WayBillInCome> dealResult = new List<WayBillInCome>();
            try
            {
                if (list == null || list.Count <= 0)
                    return result;
                List<FreightModel> modeList = new List<FreightModel>();
                foreach (WayBillInCome w in list)
                {
                    modeList.Add(new FreightModel(w));
                }
                List<Freight> freightList = this.GetFreightList(url, modeList);

                foreach (WayBillInCome w in list)
                {
                    try
                    {
                        if (w.ExpressType == ExpressTypeEnum.Economy)//经济
                        {
                            w.OperateFee = baseFee["E_OpratorFee"];
                            w.PreOperateFee = baseFee["PreE_OpratorFee"];
                        }
                        else
                        {
                            w.OperateFee = baseFee["S_OpratorFee"];
                            w.PreOperateFee = baseFee["PreS_OpratorFee"];
                        }
                        //w.SetFreight(freightList.Find(d => d.Code == w.ExpressNo));
                        dealResult.Add(w.GetWayBillInCome(freightList.Find(d => d.Code == w.ExpressNo)));
                        result.Add(w.LoadBillBy.ID);
                    }
                    catch (Exception ex)
                    {
                        string msg = string.Format("运单【{0}】更新运费失败。", w.ExpressNo);
                        LogHelper.ErrorLog(msg, ex);
                    }
                }
                WayBillInCome.BatchUpdate(dealResult);
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog("错误：", ex);
            }
            return result;
        }

        public List<Freight> GetFreightList(string url, List<FreightModel> modeList)
        {
            List<Freight> result = new List<Freight>();
            try
            {
                string resultString = WebApiClient<List<FreightModel>>.Post(url, modeList);
                string odlTitle = "<ArrayOfFreight xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/Uuch.Ucw.WebApi.Models\">";
                string newTitle = "<ArrayOfFreight xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">";
                resultString = resultString.Replace(odlTitle, newTitle);

                object obj = Deserialize(typeof(List<Freight>), resultString);
                if (obj != null)
                    result = obj as List<Freight>;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

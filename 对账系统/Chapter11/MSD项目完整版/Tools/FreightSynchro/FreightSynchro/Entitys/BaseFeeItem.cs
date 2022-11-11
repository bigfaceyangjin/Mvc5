using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightSynchro.Entitys
{
    public class BaseFeeItem : BaseEntity<BaseFeeItem, int>
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string FullName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual decimal Fee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual int BusinessType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string CreateUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual int VoidFlag { get; set; }

        
        /// <summary>
        /// 获取经济快递操作费和标准快递操作费
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static Dictionary<string, decimal> GetBySE()
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            result.Add("S_OpratorFee",0);
            result.Add("E_OpratorFee",0);
            result.Add("PreE_OpratorFee",0);
            result.Add("PreS_OpratorFee",0);
            var query = NHibernateSession.CreateSQLQuery("SELECT DISTINCT `Code`,Fee FROM Base_FeeItem where Code IN ('S_OpratorFee','E_OpratorFee','PreE_OpratorFee','PreS_OpratorFee') AND VoidFlag=0;");
            foreach (object[] item in query.List<object[]>())
            {
                if (result.Keys.Contains(item[0].ToString()))
                {
                    decimal fee = 0;
                    decimal.TryParse(item[1].ToString(), out fee);
                    result[item[0].ToString()] = fee;
                }
            }
            return result;
        } 
    }
}

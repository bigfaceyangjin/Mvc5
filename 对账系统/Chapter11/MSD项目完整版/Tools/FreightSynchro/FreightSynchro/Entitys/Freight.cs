using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightSynchro.Entitys
{
    public class Freight
    {
        /// <summary>
        /// 运单号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 发货地
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 收货地
        /// </summary>
        public string To { get; set; }

        public List<FreightItem> List { get; set; }
    }

    public class FreightItem
    {
        public string ShipCode { get; set; }
        /// <summary>
        /// 首重
        /// </summary>
        public decimal FirstWeight { get; set; }
        /// <summary>
        /// 续重
        /// </summary>
        public decimal SubWeight { get; set; }
        /// <summary>
        /// 首重金额
        /// </summary>
        public decimal FirstPrice { get; set; }
        /// <summary>
        /// 续重金额
        /// </summary>
        public decimal SubPrice { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Freight { get; set; }
    }
}

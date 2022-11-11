using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreightSynchro.Entitys
{
    public class ExpressNoExceptionDetail : BaseEntity<ExpressNoExceptionDetail, int>
    {
        /// <summary>
        /// 运单号
        /// </summary>
        public virtual string ExpressNo { get; set; }
        /// <summary>
        /// 异常类型
        /// </summary>
        public virtual ExceptionTypeEnum ExceptionType { get; set; }
        /// <summary>
        /// 异常说明
        /// </summary>
        public virtual string ExceptionMsg { get; set; }
        /// <summary>
        /// 提单ID
        /// </summary>
        public virtual string LoadBillNO { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public virtual int CustomerID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        public static void Save(WayBillInCome w, string msg)
        {
            ExpressNoExceptionDetail exDetail = new ExpressNoExceptionDetail()
            {
                ExpressNo = w.ExpressNo,
                ExceptionType = ExceptionTypeEnum.WayBillFeeException,
                ExceptionMsg = msg,
                LoadBillNO = w.LoadBillBy.LoadBillNum,
                CustomerID = w.CustomerID,
                CreateTime = DateTime.Now
            };
            exDetail.Save();
        }
    }
}

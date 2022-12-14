using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using log4net;

namespace B2CSynchro.Jobs
{
    /// <summary>
    /// 根据提单ID取接口数据 同步更新到提单收入表
    /// </summary>
    public class BillWarehouseJob : IJob
    {
        private static readonly ILog logger = log4net.LogManager.GetLogger("BillWarehouseJobLog");

        public void Execute(IJobExecutionContext context)
        {
            int sourceCount = 0;
            string guid = Guid.NewGuid().ToString();
            logger.Info(string.Format("任务名:“{0}”,唯一标识符:“{1}”开始运行", this.GetType().Name, guid));
            try
            {
                sourceCount = DBHelper.Entitys.LoadBillInfo.SynBillInComeNew();
                logger.Info(string.Format("任务名:“{0}”,唯一标识符:“{1}”运行完毕,共获取{2}条数据", this.GetType().Name, guid, sourceCount));
                return;
            }
            catch (Exception e)
            {
                logger.Error(string.Format("任务名:“{0}”,唯一标识符:“{1}”任务出现异常", this.GetType().Name, guid), e);
            }
            logger.Info(string.Format("任务名:“{0}”,唯一标识符:“{1}”运行异常,详情请见异常", this.GetType().Name, guid));
        }
    }
}

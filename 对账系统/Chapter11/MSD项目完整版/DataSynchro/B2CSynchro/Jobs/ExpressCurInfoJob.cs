using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using DBHelper;
using DBHelper.Entitys;
using NHibernate;
using log4net;

namespace B2CSynchro.Jobs
{
    public class ExpressCurInfoJob : IJob
    {
        private static readonly ILog logger = log4net.LogManager.GetLogger("ECLog");

        public void Execute(IJobExecutionContext context)
        {
            int sourceCount = 0;
            string guid=Guid.NewGuid().ToString();
            logger.Info(string.Format("任务名:“{0}”,唯一标识符:“{1}”开始运行",this.GetType().Name,guid));
            try
            {
                sourceCount=DBHelper.Entitys.ExpressCurInfo.Synchro();
                logger.Info(string.Format("任务名:“{0}”,唯一标识符:“{1}”运行完毕,共获取{2}条数据", this.GetType().Name, guid, sourceCount));
                return;
            }
            catch (Exception e)
            {
                logger.Error(string.Format("任务名:“{0}”,唯一标识符:“{1}”任务出现异常", this.GetType().Name,guid), e);
            }
            logger.Info(string.Format("任务名:“{0}”,唯一标识符:“{1}”运行异常,详情请见异常", this.GetType().Name, guid));
        }
    }
}

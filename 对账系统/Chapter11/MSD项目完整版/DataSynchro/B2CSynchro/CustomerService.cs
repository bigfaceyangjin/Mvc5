using NHibernate.Mapping;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using B2CSynchro.Jobs;


namespace B2CSynchro
{
    partial class CustomerService : ServiceBase
    {
        IScheduler scheduler = null;
        public CustomerService()
        {
            InitializeComponent();
            //工厂
            ISchedulerFactory factory = new StdSchedulerFactory();
            //启动
            scheduler = factory.GetScheduler();
            scheduler.GetJobGroupNames();
            ////描述工作
            //IJobDetail jobDetail = new JobDetailImpl("CustomerJob", null, typeof(ExpressCurInfoJob));
            ////触发器
            //ISimpleTrigger trigger = new SimpleTriggerImpl("CustomerJob", null, DateTime.Now, null, SimpleTriggerImpl.RepeatIndefinitely, TimeSpan.FromSeconds(1));
            ////执行
            //scheduler.ScheduleJob(jobDetail, trigger);
            var path = string.Format("{0}log4net.xml", AppDomain.CurrentDomain.BaseDirectory);
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            scheduler.Start();
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            scheduler.Shutdown();
        }

        protected override void OnPause()
        {
            scheduler.PauseAll();
        }

        protected override void OnContinue()
        {
            scheduler.ResumeAll();
        }
    }
}

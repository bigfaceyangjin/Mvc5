using System.Collections.Generic;
using System.ServiceProcess;
using System.Linq;
using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using B2CSynchro.Jobs;

namespace B2CSynchro
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            //try
            //{
            //    var path = string.Format("{0}log4net.xml", AppDomain.CurrentDomain.BaseDirectory);
            //    log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
            //    IScheduler scheduler = null;
            //    //工厂
            //    ISchedulerFactory factory = new StdSchedulerFactory();
            //    //启动
            //    scheduler = factory.GetScheduler();
            //    scheduler.Start();
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

            //var path = string.Format("{0}log4net.xml", AppDomain.CurrentDomain.BaseDirectory);
            //log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
            //B2CSynchro.Jobs.WayBillJob w = new WayBillJob();
            //w.Execute(null);

            //var path = string.Format("{0}log4net.xml", AppDomain.CurrentDomain.BaseDirectory);
            //log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
            //B2CSynchro.Jobs.OrderCostJob w = new OrderCostJob();
            //w.Execute(null);

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new CustomerService() 
            };
            ServiceBase.Run(ServicesToRun);

            //List<Task> li = new List<Task>();
            //for (int i = 0; i < 20; i++)
            //{
            //    //li.Add(new Task(() => { DBHelper.Entitys.ExpressCurInfo.Insert(); }));
            //    li.Add(new Task(() => { System.Threading.Thread.Sleep(6000); DBHelper.Entitys.LoadBillInfo.GetLoadBillInComeTable(); }));
            //}
            //foreach (var item in li)
            //{
            //    item.Start();
            //}
            //Task.WaitAll(li.ToArray());
        }
    }
}

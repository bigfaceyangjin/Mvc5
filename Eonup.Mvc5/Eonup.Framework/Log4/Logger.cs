using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Eonup.Framework.Log4
{
	public class Logger
	{
		static Logger()
		{
			XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"CfgFiles/log4.net.xml")));
			ILog logger = LogManager.GetLogger(typeof(Logger));
			logger.Info("系统初始化Log4Net");
		}
		ILog logger = null;
		public Logger(Type type)
		{
			logger = LogManager.GetLogger(type);
		}
		public void Info(string msg)
		{
			this.logger.Info(msg);
		}
		public void Error(string msg)
		{
			this.logger.Error(msg);
		}
		public void Warn(string msg)
		{
			this.logger.Warn(msg);
		}
		public void Fatal(string msg)
		{
			this.logger.Fatal(msg);
		}
	}
}

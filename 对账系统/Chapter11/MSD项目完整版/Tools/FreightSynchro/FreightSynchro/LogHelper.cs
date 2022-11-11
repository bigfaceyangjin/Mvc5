using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace FreightSynchro
{
    public class LogHelper
    {
        private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");

        private static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        public static void WriteLog(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }
        /// <summary>
        /// 错误记录
        /// </summary>
        /// <param name="info">附加信息</param>
        /// <param name="ex">错误</param>
        public static void ErrorLog(string info, Exception ex)
        {
            if (!string.IsNullOrEmpty(info) && ex == null)
            {
                logerror.ErrorFormat("附加信息：{0}\r\n", new object[] { info });
            }
            else if (!string.IsNullOrEmpty(info) && ex != null)
            {
                string errorMsg = BeautyErrorMsg(ex);
                logerror.ErrorFormat("附加信息：{0}\r\n{1}", new object[] { info, errorMsg });
            }
            else if (string.IsNullOrEmpty(info) && ex != null)
            {
                string errorMsg = BeautyErrorMsg(ex);
                logerror.Error(errorMsg);
            }
        }
        /// <summary>
        /// 美化错误信息
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>错误信息</returns>
        private static string BeautyErrorMsg(Exception ex)
        {
            string errorMsg = string.Format("异常类型：{0} \r\n异常信息：{1} \r\n堆栈调用：{2}", new object[] { ex.GetType().Name, ex.Message, ex.StackTrace });
            return errorMsg;
        }
    }
}

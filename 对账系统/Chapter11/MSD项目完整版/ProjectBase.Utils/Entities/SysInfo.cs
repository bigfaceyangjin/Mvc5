/* ==============================================================================
   * 功能描述：SysInfo  
   * 创 建 者：Zouqj
   * 创建日期：2015/5/29 12:21:00
   ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ProjectBase.Utils.Entities
{
    /// <summary>
    /// 系统配置信息
    /// </summary>
    public class SysInfo
    {
        /// <summary>
        /// 服务器时间
        /// </summary>
        public DateTime ServerTime
        {
            get
            {
                return Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }

        /// <summary>
        /// 版本
        /// </summary>
        public string Versions { get; set; }

        /// <summary>
        /// 服务器IP地址
        /// </summary>
        public string ServerIP { get; set; }

        /// <summary>
        /// 站点物理路径
        /// </summary>
        public string PhysicalPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        /// <summary>
        /// 是否开启客户端验证
        /// </summary>
        public string IsClientValidationEnabled { get; set; }

        /// <summary>
        /// 是否记录操作日志（true:是,false:否）
        /// </summary>
        public string IsLogEnabled { get; set; }

        /// <summary>
        /// 服务器IIS版本
        /// </summary>
        public string ServiceIIS { get; set; }

        /// <summary>
        /// 服务器.NET解释引擎版本
        /// </summary>
        public string ServiceNetVersion
        {
            get
            {
                return Environment.Version == null ? string.Empty : Environment.Version.ToString();
            }
        }

        /// <summary>
        /// 服务器操作系统
        /// </summary>
        public string ServiveSystem
        {
            get
            {
                return Environment.OSVersion.ToString();
            }
        }

        /// <summary>
        /// 网站端口号
        /// </summary>
        public string ServicePort { get; set; }


        public static SysInfo SysInfoModel;

        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <returns></returns>
        public SysInfo GetSysConfigInfo()
        {
            SysInfo info = new SysInfo();
            info.Versions = ConfigurationManager.AppSettings["Version"] == null ? string.Empty : ConfigurationManager.AppSettings["Version"].ToString();

            bool isLogEnabled;
            bool.TryParse(ConfigurationManager.AppSettings["LogEnabled"], out isLogEnabled);
            info.IsLogEnabled = isLogEnabled == true ? "是" : "否";

            bool isClientValidationEnabled;
            bool.TryParse(ConfigurationManager.AppSettings["ClientValidationEnabled"], out isClientValidationEnabled);
            info.IsClientValidationEnabled = isClientValidationEnabled == true ? "是" : "否";

            return info;
        }

        public SysInfo GetSysInfo()
        {
            SysInfo info = new SysInfo();
            info = SysInfoModel==null?GetSysConfigInfo():SysInfoModel;
            info.ServerIP = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            info.ServicePort = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            info.ServiceIIS = HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];

            return info;
        }
    }
}

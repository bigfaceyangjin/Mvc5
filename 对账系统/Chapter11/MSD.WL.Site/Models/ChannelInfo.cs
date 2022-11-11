/****************************************************************************
 *Copyright (c) 2016 All Rights Reserved.
 *CLR版本： 4.0.30319.42000
 *机器名称：DESKTOP-V7CFIC3
 *公司名称：
 *命名空间：MSD.WL.Site.Models
 *文件名：  ChannelInfo
 *版本号：  V1.0.0.0
 *唯一标识：c6a961a7-8c73-48cf-98e0-8bbf3c857bbb
 *当前的用户域：DESKTOP-V7CFIC3
 *创建人：  zouqi
 *电子邮箱：zouyujie@126.com
 *创建时间：2016/10/2 22:34:30

 *描述：
 *
 *=====================================================================
 *修改标记
 *修改时间：2016/10/2 22:34:30
 *修改人： zouqi
 *版本号： V1.0.0.0
 *描述：
 *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSD.WL.Site.Models
{
    public class ChannelInfo
    {
        public virtual int ID { get; set; }
        public virtual string ChannelStyle { get; set; }
        public virtual string ChannelCode { get; set; }
        public virtual string CnName { get; set; }
        public virtual string EnName { get; set; }
        public virtual string Status { get; set; }
    }
}
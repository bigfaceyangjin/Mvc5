/* ==============================================================================
   * 功能描述：ChannelInfo  
   * 创 建 者：Zouqj
   * 创建日期：2015/5/12 18:50:32
   ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

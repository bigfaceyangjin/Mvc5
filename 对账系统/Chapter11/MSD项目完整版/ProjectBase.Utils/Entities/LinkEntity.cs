/* ==============================================================================
   * 功能描述：LinkEntity  
   * 创 建 者：Zouqj
   * 创建日期：2015/7/1 14:39:30
   ==============================================================================*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBase.Utils.Entities
{
   public class LinkEntity
    {
       public string LinkText { get; set; }
       public string LinkUrl { get; set; }
    }
   public class LinkEntityExt
   {
       public static List<LinkEntity> ForFileLength(DirectoryInfo directory)
       {
           List<LinkEntity> list = new List<LinkEntity>();

           DirectoryInfo[] directorys = directory.GetDirectories();
           FileInfo[] files;
           foreach (DirectoryInfo di in directorys)
           {
               ForFileLength(di);
           }
           files = directory.GetFiles();
           string str = "/Excel/";
           foreach (FileInfo file in files)
           {
               LinkEntity entity = new LinkEntity { LinkText = file.Name, LinkUrl =str+file.Name };
               list.Add(entity);
           }

           return list;
       }
   }
}

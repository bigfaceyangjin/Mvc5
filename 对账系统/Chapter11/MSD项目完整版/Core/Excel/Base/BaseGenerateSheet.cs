using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Excel
{
    public abstract class BaseGenerateSheet
    {
        public string SheetName { set; get; }

        public IWorkbook Workbook { get; set; }

        public virtual void GenSheet(ISheet sheet)
        {

        }
    }
}

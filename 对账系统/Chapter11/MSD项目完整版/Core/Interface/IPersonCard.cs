using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IPersonCard
    {
        Result ValidPersonCard(string userName, string key, string cardNo, string name);
    }

    public class Result
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public bool IsCharge { get; set; }
    }
}

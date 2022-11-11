using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBase.Utils.Logs
{
    public class LogMessage
    {
        public LogMessage(
            int operatorID,
            string operand,
            int ActionType,
            string message,
            string ip,
            string machineName,
            string browser
            )
        {
            this.ActionType = ActionType;
            this.Operator = operatorID;
            this.Message = message;
            this.Operand = operand;
            this.IP = ip;
            this.Browser = browser;
            this.MachineName = machineName;
        }

        public SaveAction ActionType { get; set; }
    }
}

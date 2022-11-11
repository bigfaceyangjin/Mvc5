using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectBase.Data;

namespace ProjectBase.Utils.Logs
{
    public class LogMessage
    {
        public LogMessage(object operatorID, SaveAction actionType, Type dataType, string message, object domainID)
        {
            ActionType = actionType;
            OperatorID = operatorID;
            Message = message;
            DataType = dataType;
            DomainID = domainID;
        }

        public SaveAction ActionType { get; set; }

        public object OperatorID { get; set; }

        public string Message { get; set; }

        public Type DataType { get; set; }

        public object DomainID { get; set; }

        public override string ToString()
        {
            return string.Format("用户(ID:{0}){1}了类型为{2}(ID:{3})的对象,{4}", OperatorID, ActionType, DataType, DomainID, Message);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PongServer.Util
{

    public class LogMessage
    {
        public LogCategory LogCategory { get; set; }
        public string Id { get; set; }
        public string Message { get; set; }

        public LogMessage(LogCategory logCategory, string id, string message)
        {
            LogCategory = logCategory;
            Id = id;
            Message = message;
        }
    }
}

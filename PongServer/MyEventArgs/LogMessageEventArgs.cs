using PongServer.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace PongServer.MyEventArgs
{
    public class LogMessageEventArgs : EventArgs
    {
        public LogMessage LogMessage { get; set; }

        public LogMessageEventArgs(LogMessage logMessage)
        {
            LogMessage = logMessage;
        }
    }
}

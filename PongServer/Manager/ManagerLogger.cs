using PongServer.MyEventArgs;
using PongServer.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace PongServer.Manager
{
    public class ManagerLogger
    {
        private List<LogMessage> logMessages;
        public event EventHandler<LogMessageEventArgs> NewLogMessageEvent;

        public LogCategory LogMode;

        public ManagerLogger(LogCategory logMode)
        {
            LogMode = logMode;
            logMessages = new List<LogMessage>();
        }

        public void AddLogMessage(LogMessage logMessage)
        {
            // LogMode filter Debug > Info > Warn > Error > Fatal
            logMessages.Add(logMessage);

            if(NewLogMessageEvent != null)
            {
                NewLogMessageEvent(this, new LogMessageEventArgs(logMessage));
            }
        }

        public void AddLogMessage(LogCategory logCategory, string id, string message)
        {
            AddLogMessage(new LogMessage(logCategory, id, message));
        }
    }
}

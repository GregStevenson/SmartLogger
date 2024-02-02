using System.Collections.Generic;
using NLog;

namespace SmartLogger
{
    public class Rule
    {
        string _name;
        bool _enabled;
        int _priority;
        public Rule()
        {
            RuleAreaItems = new List<RuleAreaItem>();
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        public int Priority
        {
            get => _priority;
            set => _priority = value;
        }

        public List<RuleAreaItem> RuleAreaItems;

        private static SmartLogger.Level LogLevelToLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Off:
                    return SmartLogger.Level.Off;
                case LogLevel.Trace:
                    return SmartLogger.Level.Trace;
                case LogLevel.Debug:
                    return SmartLogger.Level.Debug;
                case LogLevel.Info:
                    return SmartLogger.Level.Info;
                case LogLevel.Warn:
                    return SmartLogger.Level.Warn;
                case LogLevel.Fatal:
                    return SmartLogger.Level.Fatal;
            }
            return SmartLogger.Level.Standby;
        }


        public void SetLoggingLevels(LogLevel loggerLevel, LogLevel detailLevel)
        {
            Enabled = true;
            foreach (RuleAreaItem item in RuleAreaItems)
            {
                item.LoggingLevel = LogLevelToLevel(loggerLevel);
                item.DetailLevel = LogLevelToLevel(detailLevel);

            }

        }


    }
}

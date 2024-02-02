

using Gurock.SmartInspect;
using SmartLogger;
//        private static readonly NLog.Logger Logger = NLog.LogManager.GetLogger("application");

namespace NLog
{
    public enum LogLevel { 
        Off, 
        Trace, 
        Debug, 
        Info, 
        Warn, 
        Fatal 
    }
    public class Logger : HyperAreaBase
    {
        public Logger(Session session) : base(session)
        {

        }
    }

    //public static class RuleExtensions
    //{

    //    private static SmartLogger.Level LogLevelToLevel(LogLevel level)
    //    {
    //        switch (level)
    //        {
    //            case LogLevel.Off:
    //                return SmartLogger.Level.Off;
    //              case LogLevel.Trace:
    //                return SmartLogger.Level.Trace;
    //            case LogLevel.Debug:
    //                return SmartLogger.Level.Debug;
    //            case LogLevel.Info:
    //                return SmartLogger.Level.Info;
    //            case LogLevel.Warn:
    //                return SmartLogger.Level.Warn;
    //            case LogLevel.Fatal:
    //                return SmartLogger.Level.Fatal;
    //        }
    //        return SmartLogger.Level.Standby;
    //    }

    //}

    //public class DummyConfiguration
    //{

    //    public Rule FindRuleByName(string name)
    //    {
    //        return Configuration.FindRuleByName(name);
    //    }
    //}
    public sealed class LogManager
    {
        public static Logger GetLogger(string logName)

        {
            return HyperLogger.MakeHyperLog(logName).Logger();
        }

        public static SmartLogger.Configuration Configuration { get { return HyperLogger.HyperConfiguration(); } }
        public static void ReconfigExistingLoggers()
        {
            HyperLogger.ReapplyRules();
        }
        public static void Shutdown()
        {

        }

    }

}


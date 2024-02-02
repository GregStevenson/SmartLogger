using Gurock.SmartInspect;


using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace SmartLogger
{
    public class HyperArea : HyperAreaBase
    {

        public HyperArea(SmartInspect parent, Area area) : base(parent, area)
        {

        }
    }
    public class HyperAreaBase
    {
        Session _session;
        bool _traceable;
        public HyperAreaBase(SmartInspect parent, Area area)
        {
            _session = new Session(parent, area.Name);
            UpdateFromArea(area);
        }
        public void UpdateFromArea(Area area)
        {
            _session.Color = ColorStringToColor(area.AreaColor);
        }
        public HyperAreaBase(Session session)
        {
            _session = session;

        }
        public string Name { get { return _session.Name; } }
        public Session SiSession { get { return _session; } }

        public bool IsInfoEnabled { get { return _session.IsOn(Gurock.SmartInspect.Level.Message); } }

        public void SetSessionLevel(Level loggerLevel, Level detailLevel)
        {
            Bootstrap.Log.LogColored(Color.PaleGreen, $"!           Session [{_session.Name}] Log: {loggerLevel.ToString()}  Detail: {detailLevel.ToString()}");
            _session.Active = IsEnabled(loggerLevel) || IsEnabled(detailLevel);
            _session.Level = XlatLevel(loggerLevel);
            _traceable = IsTraceable(loggerLevel) || IsTraceable(detailLevel);
            if (IsEnabled(detailLevel) && !_session.IsOn(XlatLevel(detailLevel)))
            {
                _session.Level = XlatLevel(detailLevel);
            }
            Bootstrap.Log.LogColored(Color.LightCyan, $"!           Session [{_session.Name}] Active: {_session.Active.ToString()}  Level: {_session.Level.ToString()}");

        }


    public static Color ColorStringToColor(string colorString)
        {
            try
            {
#if NETFRAMEWORK
                return ColorTranslator.FromHtml(colorString);
#else
                return Color.FromName(colorString);
#endif

            }
            catch
            {
                return Color.Black; // indicates unknown color designation
            }
        }
        public static Gurock.SmartInspect.Level XlatLevel(Level level)
        {
            switch (level)
            {
                case Level.Trace:
                    { return Gurock.SmartInspect.Level.Debug; }
                case Level.Debug:
                    { return Gurock.SmartInspect.Level.Debug; }
                case Level.Verbose:
                    { return Gurock.SmartInspect.Level.Verbose; }
                case Level.Info:
                    { return Gurock.SmartInspect.Level.Message; }
                case Level.Message:
                    { return Gurock.SmartInspect.Level.Message; }
                case Level.Warning:
                    { return Gurock.SmartInspect.Level.Warning; }
                case Level.Warn:
                    { return Gurock.SmartInspect.Level.Warning; }
                case Level.Error:
                    { return Gurock.SmartInspect.Level.Error; }
                case Level.Fatal:
                    { return Gurock.SmartInspect.Level.Fatal; }
                case Level.Control:
                    { return Gurock.SmartInspect.Level.Control; }
                case Level.Off:
                    { return Gurock.SmartInspect.Level.Debug; }
                case Level.Standby:
                    { return Gurock.SmartInspect.Level.Debug; }
            }
            return Gurock.SmartInspect.Level.Debug;
        }

        private static bool IsEnabled(Level level)
        {
            return !((level == Level.Off) || (level == Level.Standby));
        }
        private static bool IsTraceable(Level level)
        {
            return (level == Level.Trace);
        }
        public void ClearAll()
        {
            _session.ClearAll();
        }

        public void Log(string label, string value, Level level = Level.Debug)
        {
            _session.LogString(XlatLevel(level), label, value);
        }
        public void LogString(string label, string value, Level level = Level.Debug)
        {
            _session.LogString(XlatLevel(level), label, value);
        }
        public void Log(string label, int value, Level level = Level.Debug)
        {
            _session.LogInt(XlatLevel(level), label, value);
        }
        public void LogInt(string label, int value, Level level = Level.Debug)
        {
            _session.LogInt(XlatLevel(level), label, value);
        }
        public void Log(string label, bool value, Level level = Level.Debug)
        {
            _session.LogBool(XlatLevel(level), label, value);
        }
        public void LogBool(string label, bool value, Level level = Level.Debug)
        {
            _session.LogBool(XlatLevel(level), label, value);
        }
        public void Log(string label, byte value, Level level = Level.Debug)
        {
            _session.LogByte(XlatLevel(level), label, value);
        }
        public void LogByte(string label, byte value, Level level = Level.Debug)
        {
            _session.LogByte(XlatLevel(level), label, value);
        }
        public void Log(string label, long value, Level level = Level.Debug)
        {
            _session.LogLong(XlatLevel(level), label, value);
        }
        public void LogLong(string label, long value, Level level = Level.Debug)
        {
            _session.LogLong(XlatLevel(level), label, value);
        }
        public void Log(string label, DateTime value, Level level = Level.Debug)
        {
            _session.LogDateTime(XlatLevel(level), label, value);
        }
        public void LogDateTime(string label, DateTime value, Level level = Level.Debug)
        {
            _session.LogDateTime(XlatLevel(level), label, value);
        }
        public void Log(string label, char value, Level level = Level.Debug)
        {
            _session.LogChar(XlatLevel(level), label, value);
        }
        public void LogChar(string label, char value, Level level = Level.Debug)
        {
            _session.LogChar(XlatLevel(level), label, value);
        }
        public void Log(string label, float value, Level level = Level.Debug)
        {
            _session.LogFloat(XlatLevel(level), label, value);
        }
        public void LogFloat(string label, float value, Level level = Level.Debug)
        {
            _session.LogFloat(XlatLevel(level), label, value);
        }
        public void Log(string label, double value, Level level = Level.Debug)
        {
            _session.LogDouble(XlatLevel(level), label, value);
        }
        public void LogDouble(string label, double value, Level level = Level.Debug)
        {
            _session.LogDouble(XlatLevel(level), label, value);
        }
        public void Log(string label, short value, Level level = Level.Debug)
        {
            _session.LogShort(XlatLevel(level), label, value);
        }
        public void LogShort(string label, short value, Level level = Level.Debug)
        {
            _session.LogShort(XlatLevel(level), label, value);
        }

        public void LogDebug(string msg)
        {
            _session.LogDebug(msg);
        }
        public void Debug(string msg)
        {
            _session.LogDebug(msg);
        }
        public void Verbose(string msg, Color color)
        {
            _session.LogColored(Gurock.SmartInspect.Level.Verbose, color, msg);
        }
        public void Verbose(string msg)
        {
            _session.LogVerbose(msg);
        }
        public void Debug(string msg, Color color)
        {
            _session.LogColored(Gurock.SmartInspect.Level.Debug, color, msg);
        }
        public void DebugEx(string msg, Color color, SmartLogger.Level level= SmartLogger.Level.Debug)
        {
            _session.LogColored(XlatLevel(level), color, msg);
        }
        public void Trace(string msg)
        {
             if (_traceable)
            {
                _session.LogDebug(msg);
                
            }
        }
        public void LogTrace(string msg)
        {
            if (_traceable)
            {
               _session.LogDebug(msg);  
            }
           
        }
        public void Error(string msg)
        {
            _session.LogError(msg);
        }
        public void LogError(string msg)
        {
            _session.LogError(msg);
        }
        public void Error(Exception e, string msg = "")
        {
            if (_session.IsOn(Gurock.SmartInspect.Level.Error))
            {
                _session.LogException(msg, e);
            }
        }
        public void SoftException(Exception e, string msg = "", Level level = Level.Debug)
        {
            if (_session.IsOn(XlatLevel(level)))
            {
                _session.LogException(msg, e);
            }
        }

        public void Warn(Exception e, string msg = "")
        {
            if (_session.IsOn(Gurock.SmartInspect.Level.Warning))
            {
                _session.LogException(msg, e);
            }
        }
        public void Info(Exception e, string msg = "")
        {
            if (_session.IsOn(Gurock.SmartInspect.Level.Message))
            {
                _session.LogException(msg, e);
            }
        }
        //public void Info(string format, string key, string value)
        //{
        //    string s;
        //    s = format.Replace("{key}", "{0}");
        //    s = s.Replace("{value}", "{1}");
        //    try
        //    {
        //        _session.LogMessage(string.Format(s, key, value));
        //    }

        //    catch (Exception ex)
        //    {
        //        _session.LogError($"{format} is not a supported format string, but this is your Key: {key}, Value: {value} pairing.");
        //    }

        //}
        //public void Info(string format, string key, object value)
        //{
        //    Info(format, key, value.ToString());
        //}
        public void Warn(string msg)
        {
            _session.LogWarning(msg);
        }
        public void Warning(string msg)
        {
            _session.LogWarning(msg);
        }
        public void LogWarning(string msg)
        {
            _session.LogWarning(msg);
        }
        public void Message(string msg)
        {
            _session.LogMessage(msg);
        }
        public void Info(string msg)
        {
            _session.LogMessage(msg);
        }
        public void Fatal(string msg)
        {
            _session.LogFatal(msg);
        }
        public void Fatal(Exception e, string msg = "")
        {
            _session.LogException(msg, e);
        }
        public void Exception(Exception e, string msg = "")
        {
            _session.LogException(msg, e);
        }
        public void Trace(string msg, Color color)
        {
            if (_traceable)
            {
                _session.LogColored(Gurock.SmartInspect.Level.Debug, color, msg);
                
            }
        }
        public void Message(string msg, Color color)
        {
            _session.LogColored(Gurock.SmartInspect.Level.Message, color, msg);
        }
        public void Info(string msg, Color color)
        {
            _session.LogColored(Gurock.SmartInspect.Level.Message, color, msg);
        }
        public void Warn(string msg, Color color)
        {
            _session.LogColored(Gurock.SmartInspect.Level.Warning, color, msg);
        }
        public void Warning(string msg, Color color)
        {
            _session.LogColored(Gurock.SmartInspect.Level.Warning, color, msg);
        }
        public void Error(string msg, Color color)
        {
            _session.LogColored(Gurock.SmartInspect.Level.Error, color, msg);
        }
        public void Fatal(string msg, Color color)
        {
            _session.LogColored(Gurock.SmartInspect.Level.Fatal, color, msg);
        }
        public void LogSource(string msg, string source, ViewerId id, Level level = Level.Debug)
        {
            _session.LogCustomText(XlatLevel(level), msg, source, LogEntryType.Source, id);
        }

        public void Python(string msg, string source, Level level = Level.Debug)
        {
            LogSource(msg, source, ViewerId.PythonSource, level);
        }
        public void XML(string msg, string source, Level level = Level.Debug)
        {
            LogSource(msg, source, ViewerId.XmlSource, level);
        }

        public void Text(string msg, string source, Level level = Level.Debug)
        {
            _session.LogText(XlatLevel(level), msg, source);
        }
        public void Binary(string msg, byte[] source, Level level = Level.Debug)
        {
            _session.LogBinary(XlatLevel(level), msg, source);
        }
        public void DataTable(string msg, DataTable source, Level level = Level.Debug)
        {
            _session.LogDataTable(XlatLevel(level), msg, source);
        }

        public void JSON(string msg, string source, Level level = Level.Debug)
        {
            if (_session.IsOn(XlatLevel(level)))
            {
                JToken parsedJson = JToken.Parse(source);
                var beautified = parsedJson.ToString(Newtonsoft.Json.Formatting.Indented);
                _session.LogSource(XlatLevel(level), msg, beautified, SourceId.JavaScript);
            }

        }
        public void Strings(string msg, IEnumerable<string> list, Level level = Level.Debug)
        {
            if (_session.IsOn(XlatLevel(level)))
            {

                var count = 0;
                ListViewerContext ctx = new ListViewerContext();
                try
                {
                    foreach (string s in list)
                    {
                        count++;
                        ctx.AppendLine(s);

                    }
                    _session.LogCustomContext(XlatLevel(level), $"{msg}({count})", LogEntryType.Text, ctx);
                }
                finally
                {
                    ctx.Dispose();
                }

            }

        }

        public void Separator(Level level = Level.Debug)
        {
            if (_session.IsOn(XlatLevel(level)))
            {
                _session.LogSeparator();
            }
            
        }
    private string FancyFormat(string format, params object[] args)
        {
            string[] chunks = format.Split('{');
            //Bootstrap.Log.LogString("Format String", format);
            //Bootstrap.Log.LogInt("Word count", chunks.Length);
            string result = "";
            string property;
            string remainder;
            string working;
            int counter = 0;
            int argCounter = 0;
            int index;
            if (chunks.Length > 0)
            {
                foreach (string chunk in chunks)
                {
                    if (counter == 0)
                    {
                        result = chunks[counter];
                    }
                    else
                    {
                        //Bootstrap.Log.LogDebug($"Argument {counter}:{args[counter - 1].ToString()}");
                        if (args[argCounter] != null)
                        {
                            result += args[argCounter].ToString();
                        }
                        else
                        {
                            result += "null";
                        }
                        argCounter++;
                        working = chunks[counter];
                        //Bootstrap.Log.LogDebug($"Working {counter}:{working}");
                        index = working.IndexOf('}');
                        //Bootstrap.Log.LogInt("Index", index);
                        //Bootstrap.Log.LogInt("Length", working.Length);
                        property = working.Substring(0, index - 1);
                        if (index < working.Length - 1)
                        {
                            index++;
                            remainder = working.Substring(index, working.Length - index);
                            //Bootstrap.Log.LogDebug($"{property}:{remainder}");
                            result += remainder;

                        }

                    }
                    //Bootstrap.Log.LogDebug($"{counter}:{result}");
                    counter++;
                }
            }
            while (argCounter < args.Length)
            {
                result += ": ";
                if (args[argCounter] != null)
                {
                    result += args[argCounter].ToString();
                }
                else
                {
                    result += "null";
                }
                argCounter++;
            }
            return result;
        }


        public void Debug(string format, params object[] args)
        {
            _session.LogMessage(FancyFormat(format, args));
        }
        public void Info(string format, params object[] args)
        {
            _session.LogMessage(FancyFormat(format, args));
        }
        public void Warn(string format, params object[] args)
        {
            _session.LogMessage(FancyFormat(format, args));
        }
        public void Error(string format, params object[] args)
        {
            _session.LogMessage(FancyFormat(format, args));
        }
        public void Error(Exception e, string format, params object[] args)
        {
            _session.LogException(FancyFormat(format, args), e);
        }
        public void Watch(string label, string value, Level level = Level.Debug)
        {
            _session.WatchString(XlatLevel(level), label, value);
        }
        public void WatchString(string label, string value, Level level = Level.Debug)
        {
            _session.WatchString(XlatLevel(level), label, value);
        }
        public void Watch(string label, int value, Level level = Level.Debug)
        {
            _session.WatchInt(XlatLevel(level), label, value);
        }
        public void WatchInt(string label, int value, Level level = Level.Debug)
        {
            _session.WatchInt(XlatLevel(level), label, value);
        }
        public void Watch(string label, bool value, Level level = Level.Debug)
        {
            _session.WatchBool(XlatLevel(level), label, value);
        }
        public void WatchBool(string label, bool value, Level level = Level.Debug)
        {
            _session.WatchBool(XlatLevel(level), label, value);
        }
        public void Watch(string label, byte value, Level level = Level.Debug)
        {
            _session.WatchByte(XlatLevel(level), label, value);
        }
        public void WatchByte(string label, byte value, Level level = Level.Debug)
        {
            _session.WatchByte(XlatLevel(level), label, value);
        }
        public void Watch(string label, long value, Level level = Level.Debug)
        {
            _session.WatchLong(XlatLevel(level), label, value);
        }
        public void WatchLong(string label, long value, Level level = Level.Debug)
        {
            _session.WatchLong(XlatLevel(level), label, value);
        }
        public void Watch(string label, DateTime value, Level level = Level.Debug)
        {
            _session.WatchDateTime(XlatLevel(level), label, value);
        }
        public void WatchDateTime(string label, DateTime value, Level level = Level.Debug)
        {
            _session.WatchDateTime(XlatLevel(level), label, value);
        }
        public void Watch(string label, char value, Level level = Level.Debug)
        {
            _session.WatchChar(XlatLevel(level), label, value);
        }
        public void WatchChar(string label, char value, Level level = Level.Debug)
        {
            _session.WatchChar(XlatLevel(level), label, value);
        }
        public void Watch(string label, float value, Level level = Level.Debug)
        {
            _session.WatchFloat(XlatLevel(level), label, value);
        }
        public void WatchFloat(string label, float value, Level level = Level.Debug)
        {
            _session.WatchFloat(XlatLevel(level), label, value);
        }
        public void Watch(string label, double value, Level level = Level.Debug)
        {
            _session.WatchDouble(XlatLevel(level), label, value);
        }
        public void WatchDouble(string label, double value, Level level = Level.Debug)
        {
            _session.WatchDouble(XlatLevel(level), label, value);
        }
        public void Watch(string label, short value, Level level = Level.Debug)
        {
            _session.WatchShort(XlatLevel(level), label, value);
        }
        public void WatchShort(string label, short value, Level level = Level.Debug)
        {
            _session.WatchShort(XlatLevel(level), label, value);
        }
        public void ClearWatches()
        {
            _session.ClearWatches();
        }
        public void ClearLog()
        {
            _session.ClearLog();
        }

        public void StackTrace(string label, Level level = Level.Debug)
        {
            _session.LogCurrentStackTrace(XlatLevel(level), label);
        }
        public void EnterMethod(string method, Level level = Level.Debug)
        {
            _session.EnterMethod(XlatLevel(level), method);
        }
        public void LeaveMethod(string method, Level level = Level.Debug)
        {
            _session.LeaveMethod(XlatLevel(level), method);
        }
    }

}

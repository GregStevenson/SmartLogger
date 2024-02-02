
using System.Drawing;
using Gurock.SmartInspect;

namespace SmartLogger
{
    public sealed class HyperLog
    {
        private HyperArea _hyperarea;
        private NLog.Logger _Logger;
        private SmartInspect _SmartInspect;
        private string _areaName;
        private Level _LoggerLevel;
        private Level _DetailLevel;

        public string Name
        {
            get { return _hyperarea.Name; }
        }
        public string LowLevel()
        {
            return _DetailLevel.ToString();
        }
        public string HighLevel()
        {
            return _LoggerLevel.ToString();
        }
        public HyperArea Ray
        {
            get { return Low; }
        }

        public HyperArea Low
        {
            get { return Detail; }
        }

        public HyperArea High
        {
            get { return Log; }
        }

        public HyperArea Log
        {
            get { return _hyperarea; }
        }
        public HyperArea Detail
        {
            get
            {
                if (_DetailLevel == Level.Standby)
                {
                    return _hyperarea;
                }
                else if (_DetailLevel == Level.Off)
                {
                    return null;
                }
                else
                if (_hyperarea.SiSession.IsOn(HyperArea.XlatLevel(_DetailLevel)))
                {
                    return _hyperarea;
                }
                else
                {
                    return null;
                }
            }
        }

        public void UpdateFromArea(Area area)
        {
            _hyperarea.UpdateFromArea(area);
        }

        private bool IsDetailLevel(Level level)
        {
            switch (level)
            {
                case Level.Trace:
                    return true;
                case Level.Debug:
                    return true;
                case Level.Verbose:
                    return true;
                case Level.Info:
                    return true;
                case Level.Message:
                    return true;
            }
            return false;
        }
        public HyperLog(SmartInspect parent, Area area)
        {
            _SmartInspect = parent;
            _areaName = area.Name;
            _hyperarea = MakeArea(area);
            _LoggerLevel = Level.Warning;
            _DetailLevel = Level.Off;
            _Logger = null;
        }

        public void SetHyperLogLevels(Level loggerLevel, Level detailLevel)
        {
            _LoggerLevel = loggerLevel;
            _DetailLevel = detailLevel;
            Bootstrap.Log.LogColored (Color.LightPink, $"!           Rule [{Name}] Log: {_LoggerLevel.ToString()}  Detail: {_DetailLevel.ToString()}");
            _hyperarea.SetSessionLevel(loggerLevel, detailLevel);  
        }

        private HyperArea MakeArea(Area area)
        {
            HyperArea hyperArea;
            hyperArea = new HyperArea(_SmartInspect, area);
            return hyperArea;
        }

        private NLog.Logger MakeLogger()
        {
            NLog.Logger logger;
            logger = new NLog.Logger(_hyperarea.SiSession);
            return logger;
        }

        public NLog.Logger Logger()
        {
            if (_Logger == null)
            {
                _Logger = MakeLogger();
            }
            return _Logger;
        }



        public HyperArea Rays(string msg)
        {
            if (msg.ToLower() == "live")
            {
                return High;

            }
            else if (msg.ToLower() == "nlog")
            {
                return High;

            }
            else
            {
                return null;

            }
        }
    }
}

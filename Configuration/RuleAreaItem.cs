

namespace SmartLogger
{
    public class RuleAreaItem
    {

        Level _loggingLevel;
        Level _detailLevel;
        string _mask;
        bool _enabled;



        public string Mask
        {
            get => _mask;
            set => _mask = value;
        }

        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        public Level LoggingLevel
        {
            get => _loggingLevel;
            set => _loggingLevel = value;
        }


        public Level DetailLevel
        {
            get => _detailLevel;
            set => _detailLevel = value;
        }


    }
}


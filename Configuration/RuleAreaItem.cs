using System.Xml;
using System.Xml.Serialization;


namespace SmartLogger
{
    public class RuleAreaItem
    {

        Level _loggingLevel;
        Level _detailLevel;
        string _mask;
        bool _enabled;



        [XmlElement("Mask")]
        public string Mask
        {
            get => _mask;
            set => _mask = value;
        }

        [XmlElement("Enabled")]
        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        [XmlElement("LoggingLevel")]
        public Level LoggingLevel
        {
            get => _loggingLevel;
            set => _loggingLevel = value;
        }


        [XmlElement("DetailLevel")]
        public Level DetailLevel
        {
            get => _detailLevel;
            set => _detailLevel = value;
        }


    }
}


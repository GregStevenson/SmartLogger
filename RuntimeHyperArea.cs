using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogger
{
    public class RuntimeHyperArea
    {

        string lowLevel;
        string highLevel;
        string level;
        string enabled;
        string color;
        string name;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Color
        {
            get => color;
            set => color = value;
        }
        public Color AreaColor
        {
            get => HyperAreaBase.ColorStringToColor(color);
          }


        public string Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        public string Level
        {
            get => level;
            set => level = value;
        }

        public string HighLevel
        {
            get => highLevel;
            set => highLevel = value;
        }
        
        public string LowLevel
        {
            get => lowLevel;
            set => lowLevel = value;
        }
        
    }
}

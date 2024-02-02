using System.Drawing;

namespace SmartLogger
{
    public class ColorManager
    {
        int _nextColor = 0;
        public ColorManager()
        {

        }
        public Color NextColor()
        {
            Color result;
            result = defaultLogColors[_nextColor];
            _nextColor++;
            if (_nextColor >= defaultLogColors.Length)
            {
                _nextColor = 0;
            }
            return result;
        }

        public static Color[] defaultLogColors =
        {
           Color.SeaShell,
           Color.PowderBlue,
           Color.LemonChiffon,
           Color.Gainsboro,
           Color.DarkSeaGreen,
           Color.Plum,
           Color.Bisque,
           Color.PaleGreen,
           Color.LightCoral,
           Color.Honeydew,
           Color.DodgerBlue,
           Color.DarkSlateBlue,
           Color.SlateGray,
           Color.Aqua,
           Color.DarkKhaki,
           Color.Chocolate,
           Color.Orange,
           Color.Pink,
           Color.IndianRed,
           Color.Olive
        };
    }
}

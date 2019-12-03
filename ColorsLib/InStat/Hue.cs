using System;
using System.Windows.Media;

namespace ColorsLib.InStat
{
    public class Hue
    {
        public Hue(string name, Color color)
        {
            Name = name;
            Color = color;
            SolidColor = new SolidColorBrush(color);
        }

        public string Name { get; }
        public Color Color { get; }
        public SolidColorBrush SolidColor { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
using System;
using System.Windows.Media;

namespace ControlsLibrary.Helpers
{
    public static class BrushHelper
    {
        public static SolidColorBrush ToSolidColorBrush(this string str)
        {
            return (SolidColorBrush)ToBrush(str);
        }

        public static Brush ToBrush(this string str)
        {
            var brushConverter = new BrushConverter();
            var brush = (SolidColorBrush)brushConverter.ConvertFrom(str);
            if (brush != null)
                return brush;

            throw new Exception("Не удалось конвертировать строку " + str + " в SolidColorBrush");
        }
    }
}
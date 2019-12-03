using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Input;

namespace ThemesLib.InStat.Convertors
{
    [ValueConversion(typeof(Key), typeof(string))]
    public class KeyToStringConvertor : IValueConverter
    {
        #region Methods

        /// <summary>
        /// System.Windows.Input.Key to String
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (value.GetType() != typeof(Key))
                throw new Exception("The input value of the converter must be of type " + typeof(Key));
           if (targetType != typeof(string))
                throw new Exception("The target type must be of type " + typeof(string));

            return ((Key) value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
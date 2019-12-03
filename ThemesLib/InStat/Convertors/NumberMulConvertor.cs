using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using static System.Double;

namespace ThemesLib.InStat.Convertors
{
    [ValueConversion(typeof(double), typeof(double))]
    public class NumberMulConvertor : IValueConverter
    {
        #region Methods

        /// <summary>
        /// value * delta (parameter)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            if (value.GetType() != typeof(double))
                throw new Exception("The input value of the converter must be of type double");
            if (parameter.GetType() != typeof(string))
                throw new Exception("The input value of the parameter must be of type string");
            if (targetType != typeof(double))
                throw new Exception("The target type must be double");


            string param = (string)parameter;
            var reg = Regex.Matches(param, @"(-?\d.\d+)|(:)|(-?\d+)");

            if (reg.Count != 3)
            {
                throw new Exception($"The converter string was expected - [num]:[default] 0.15:10 : {param}");
            }

            double inVal = (double)value;
            double delta = Parse(reg[0].Groups[0].Value, CultureInfo.InvariantCulture);

            if (Math.Abs(inVal) <= 0 || Math.Abs(delta) <= 0)
            {
                double def = Parse(reg[2].Groups[0].Value, CultureInfo.InvariantCulture);
                return def;
            }

            var res = inVal * delta;
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
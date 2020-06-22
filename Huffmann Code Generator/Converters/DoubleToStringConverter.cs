using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Huffmann_Code_Generator.Converters
{
    /// <summary>
    /// Converter zum formatierten Anzeigen von Double Werten 
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            string param = parameter as string;
            if (param != null)
                return ((double)value).ToString(param);
            throw new InvalidCastException();
        }
        public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            string amount = value as string;
            return System.Convert.ToDouble(amount);
        }
    }
    
}

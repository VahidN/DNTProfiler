using System;
using System.Globalization;
using System.Windows.Data;
using DNTProfiler.Common.JsonToolkit;

namespace DNTProfiler.Common.Converters
{
    public class ObjectToJsonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToFormattedJson();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
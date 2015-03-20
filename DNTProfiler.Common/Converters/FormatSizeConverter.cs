using System;
using System.Globalization;
using System.Windows.Data;

namespace DNTProfiler.Common.Converters
{
    public class FormatSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";

            var size = value.ToString();
            return string.IsNullOrWhiteSpace(size) ? "" : sizeToString(long.Parse(size));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        static string sizeToString(long number)
        {
            string[] suf = { " B", " KB", " MB", " GB", " TB", " PB", " EB" };
            if (number == 0)
                return "0" + suf[0];
            var bytes = Math.Abs(number);
            var place = System.Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(number) * num) + suf[place];
        }
    }
}

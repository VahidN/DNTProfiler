using System;
using System.Globalization;
using System.Windows.Data;
using DNTProfiler.Common.Models;
using DNTProfiler.Infrastructure.StackTraces;

namespace DNTProfiler.Infrastructure.Converters
{
    public class CallingLineToMethodBodyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var methodInfo = value as CallingMethodInfo;
            if (methodInfo == null)
                return string.Empty;

            return MethodDeclarations.GetMethodBody(
                methodInfo.CallingFileFullName,
                methodInfo.CallingMethod,
                methodInfo.CallingLine);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
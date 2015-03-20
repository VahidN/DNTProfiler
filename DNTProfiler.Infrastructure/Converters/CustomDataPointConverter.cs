using System;
using System.Globalization;
using System.Windows.Data;
using DNTProfiler.Infrastructure.Core;
using OxyPlot;

namespace DNTProfiler.Infrastructure.Converters
{
    public class CustomDataPointConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var hitResult = values[0] as TrackerHitResult;
            if (hitResult == null)
                return string.Empty;

            var tooltipProvider = values[1] as CustomTooltipProvider;
            if (tooltipProvider == null)
                return string.Empty;

            if (tooltipProvider.GetCustomTooltip == null)
                return hitResult.Text;

            var customTooltip = tooltipProvider.GetCustomTooltip(hitResult);
            if (string.IsNullOrWhiteSpace(customTooltip))
                return hitResult.Text;

            return hitResult.Text + Environment.NewLine + customTooltip;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
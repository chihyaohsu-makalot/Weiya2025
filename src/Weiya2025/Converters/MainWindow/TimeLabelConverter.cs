using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Weiya2025.Converters.MainWindow
{
    internal class TimeLabelConverter : IValueConverter
    {
        //to view
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return string.Empty;

            var exp = ((DateTimeOffset)value).DateTime;

            //轉成字串顯示
            return $"{(exp.Kind == DateTimeKind.Utc ? exp.ToLocalTime() : exp):yyyy-MM-dd HH:mm:ss}";
        }

        //to source
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using Avalonia.Data.Converters;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Weiya2025.Converters.MainWindow
{
    internal class ChineeseNameConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return string.Empty;

            var fullName = (string)value;
            var enPattern = new Regex(@"[A-Za-z]+");
            var matches = enPattern.Matches(value.ToString()!);
            var name = enPattern.Replace(fullName, string.Empty).Trim();

            return name;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;

namespace BookDb.Converters
{
    // class for not letting user type anything else than numbers to numeric fields
    public class NumericConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? string.Empty : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = value as string;

            if (string.IsNullOrWhiteSpace(stringValue))
                return null;

            if (int.TryParse(stringValue, out int intValue))
                return intValue;

            return System.Windows.DependencyProperty.UnsetValue;
        }
    }
}

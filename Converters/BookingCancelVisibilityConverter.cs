using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PereMaria.GestorHotel.Converters
{
    public class BookingCancelVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var isEditing = values.Length > 0 && values[0] is bool b && b;
            var status = values.Length > 1 ? values[1] as string : null;

            if (!isEditing) return Visibility.Collapsed;

            return string.Equals(status, "canceled", StringComparison.OrdinalIgnoreCase)
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return Array.Empty<object>();
        }
    }
}
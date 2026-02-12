using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PereMaria.GestorHotel.Converters
{
    public class BookingStatusToBadgeBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as string;
            var resourceKey = string.Equals(status, "canceled", StringComparison.OrdinalIgnoreCase)
                ? "Bg-Danger"
                : "Bg-Success";

            if (Application.Current != null && Application.Current.Resources.Contains(resourceKey))
            {
                return (Brush)Application.Current.Resources[resourceKey];
            }

            return string.Equals(status, "canceled", StringComparison.OrdinalIgnoreCase)
                ? Brushes.Firebrick
                : Brushes.SeaGreen;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
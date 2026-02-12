using System;
using System.Globalization;
using System.Windows.Data;

namespace PereMaria.GestorHotel.Converters
{
    public class BookingStatusToBadgeTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as string;
            return string.Equals(status, "canceled", StringComparison.OrdinalIgnoreCase)
                ? "Cancelado"
                : "Activo";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
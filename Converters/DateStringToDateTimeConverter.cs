using System;
using System.Globalization;
using System.Windows.Data;

namespace PereMaria.GestorHotel.Converters
{
    public class DateStringToDateTimeConverter : IValueConverter
    {
        private const string DateFormat = "dd/MM/yyyy";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateText && !string.IsNullOrWhiteSpace(dateText))
            {
                if (DateTime.TryParseExact(dateText, DateFormat, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var parsed))
                {
                    return parsed;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateValue)
            {
                return dateValue.ToString(DateFormat, CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }
    }
}
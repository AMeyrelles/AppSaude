﻿using System.Globalization;


namespace AppSaude.Dictionary
{
    class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}"; // Formato hh:mm
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

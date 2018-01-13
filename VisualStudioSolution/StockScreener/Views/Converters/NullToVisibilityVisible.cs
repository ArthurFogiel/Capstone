using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StockScreener.Views.Converters
{
    /// <summary>
    /// True = Visible, False = Collapsed
    /// </summary>
    public class NullToVisibilityVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

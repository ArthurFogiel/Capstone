﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StockScreener.Views.Converters
{
    /// <summary>
    /// True = Collapsed, False = Visible
    /// </summary>
    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //We don't convert back since we don't know the object
            return null;
        }
    }
}

using System;
using System.Drawing;
using System.Windows.Data;

namespace StockScreener.Views.Converters
{
    /// <summary>
    /// True = Collapsed, False = Visible
    /// </summary>
    public class PositiveToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is float)
            {
                if((float)value > 0)
                {
                    return Brushes.Green;
                }
                if( (float)value < 0)
                {
                    return Brushes.IndianRed;
                }
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

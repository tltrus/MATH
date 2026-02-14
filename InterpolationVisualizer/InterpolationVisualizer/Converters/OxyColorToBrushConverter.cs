using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using OxyPlot;

namespace InterpolationVisualizer.Converters
{
    public class OxyColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OxyColor oxyColor)
            {
                return new SolidColorBrush(Color.FromArgb(
                    oxyColor.A, oxyColor.R, oxyColor.G, oxyColor.B));
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
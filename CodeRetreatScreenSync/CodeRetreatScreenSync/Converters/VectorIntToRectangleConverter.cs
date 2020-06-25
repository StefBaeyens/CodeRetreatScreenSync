using System;
using System.Globalization;
using System.Numerics;
using Xamarin.Forms;

namespace CodeRetreatScreenSync.Converters
{
    public class VectorIntToRectangleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Vector2 vector && parameter is double size)
                return new Rectangle(vector.X, vector.Y, size, size);
            return new Rectangle(0,0,50,50);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

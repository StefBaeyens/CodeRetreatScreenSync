using System;
using System.Globalization;
using System.Numerics;
using Xamarin.Forms;

namespace CodeRetreatScreenSync.Converters
{
    public class VectorToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Vector2 vector2)
            {
                return "{" + vector2.X + ", " + vector2.Y + "}";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

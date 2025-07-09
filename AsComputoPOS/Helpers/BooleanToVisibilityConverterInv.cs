using System.Globalization;
using System.Windows.Data;

namespace TamoPOS.Helpers
{
    public class BooleanToVisibilityConverterInv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                // If parameter is "Inverse", reverse the logic
                if (parameter?.ToString() == "Inverse")
                {
                    return booleanValue ? Visibility.Collapsed : Visibility.Visible;
                }
                else
                {
                    return booleanValue ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            return Visibility.Collapsed; // Default in case of non-boolean value
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

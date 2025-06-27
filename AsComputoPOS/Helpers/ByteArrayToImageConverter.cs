using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TamoPOS.Helpers
{
    public class ByteArrayToImageConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is byte[] bytes && bytes.Length > 0)
            {
                using var stream = new MemoryStream(bytes);
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
                return image;
            }
            return null!;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack is not implemented for ByteArrayToImageConverter.");
        }
    }
}

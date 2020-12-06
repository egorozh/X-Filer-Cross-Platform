using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;

namespace ChromER.WPF.UI
{
    internal class IconPathToImageConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = new Image();
            image.Stretch = Stretch.Uniform;

            if (value is string imagePaths)
            {
                var imagePath = new FileInfo(imagePaths);

                if (imagePath.Extension.ToUpper() == ".SVG")
                {
                    var settings = new WpfDrawingSettings
                    {
                        TextAsGeometry = false,
                        IncludeRuntime = true,
                    };

                    var converter = new FileSvgReader(settings);

                    var drawing = converter.Read(imagePath.FullName);

                    if (drawing != null)
                    {
                        var drawImage = new DrawingImage(drawing);
                        image.Source = drawImage;
                    }
                }
                else
                {
                    var bitmapSource = new BitmapImage(new Uri(imagePath.FullName));
                    image.Source = bitmapSource;
                }
            }

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class YourConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
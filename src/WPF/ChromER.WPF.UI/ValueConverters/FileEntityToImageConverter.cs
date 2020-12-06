using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChromER.WPF.UI
{
    internal class FileEntityToImageConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dravingImage = new DrawingImage();

            if (!(value is FileEntityViewModel viewModel))
                return dravingImage;

            var imagePath = ChromEr.Instance.IconsManager.GetIconPath(viewModel);

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
                    return new DrawingImage(drawing);
            }
            else
            {
                var bitmapSource = new BitmapImage(new Uri(imagePath.FullName));
                return bitmapSource;
            }

            return dravingImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
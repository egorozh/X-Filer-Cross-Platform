using System;
using System.Globalization;
using System.IO;
using System.Net.WebSockets;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Explorer.Shared.ViewModels;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;

namespace ChromER.WPF.UI
{
    internal class FileEntityToImageConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dravingImage = new DrawingImage();

            if (value is DirectoryViewModel)
            {
                var resource = Application.Current.TryFindResource("FolderIconImage");

                if (resource is ImageSource directoryImageSource)
                    return directoryImageSource;
            }
            else if (value is FileViewModel fileViewModel)
            {
                var extension = new FileInfo(fileViewModel.FullName).Extension;

                var imagePath = ExtensionToImageFileConverter.GetImagePath(extension);

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
                
                var resource = Application.Current.TryFindResource("FileIconImage");

                if (resource is ImageSource fileImageSource)
                    return fileImageSource;
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
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using ChromER.Application;
using Explorer.Shared.ViewModels;
using Svg;
using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace Explorer.Avalonia.UI
{
    internal class FileEntityToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Bitmap dravingImage = null;

            if (!(value is FileEntityViewModel viewModel))
                return dravingImage;

            var imagePath = ChromEr.Instance.IconsManager.GetIconPath(viewModel);

            if (imagePath.Extension.ToUpper() == ".SVG")
            {
                var svgDocument = SvgDocument.Open(imagePath.FullName);

                if (svgDocument != null)
                {
                    var bitmap = svgDocument.Draw();

                    using var stream = new MemoryStream();

                    bitmap.Save(stream, ImageFormat.Png);

                    stream.Seek(0, SeekOrigin.Begin);

                    return new Bitmap(stream);
                }
            }
            else
            {
                return new Bitmap(imagePath.FullName);
            }

            return dravingImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
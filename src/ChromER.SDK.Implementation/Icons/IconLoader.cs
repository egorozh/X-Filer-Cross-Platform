using Avalonia.Media.Imaging;
using Svg;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;

namespace ChromER.SDK
{
    public class IconLoader : IIconLoader
    {
        private readonly Dictionary<string, Bitmap> _cash = new();

        private readonly IIconPathProvider _iconPathProvider;

        public IconLoader(IIconPathProvider iconPathProvider)
        {
            _iconPathProvider = iconPathProvider;
        }

        public Bitmap? GetIcon(FileEntityViewModel viewModel)
        {
            var fileInfo = _iconPathProvider.GetIconPath(viewModel);
            var path = fileInfo.FullName;

            if (_cash.ContainsKey(path))
                return _cash[path];

            Bitmap? source = null;

            if (fileInfo.Extension.ToUpper() == ".SVG")
            {
                var svgDocument = SvgDocument.Open(fileInfo.FullName);

                if (svgDocument != null)
                {
                    var bitmap = svgDocument.Draw();

                    using var stream = new MemoryStream();

                    bitmap.Save(stream, ImageFormat.Png);

                    stream.Seek(0, SeekOrigin.Begin);

                    source = new Bitmap(stream);
                    _cash.Add(path, source);
                }
            }
            else
            {
                source = new Bitmap(fileInfo.FullName);
                _cash.Add(path, source);
            }
            
            return source;
        }
    }
}
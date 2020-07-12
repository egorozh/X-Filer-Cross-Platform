using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChromER
{
    internal class ExtensionToImageFileConverter
    {
        private Dictionary<string, FileInfo> _icons;

        public ExtensionToImageFileConverter()
        {
            var applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var iconsDirectory = new DirectoryInfo(Path.Combine(applicationDirectory, "Resources", "Icons"));

            _icons = iconsDirectory
                .GetFiles()
                .ToDictionary(fi => GetNameWithoutExtension(fi.Name));
        }

        public FileInfo GetImagePath(string extension)
        {
            if (_icons.ContainsKey(extension.ToUpper()))
                return _icons[extension.ToUpper()];

            return _icons["._BLANK"];
        }

        private string GetNameWithoutExtension(string fileName)
        {
            // 7z.svg

            var parts = fileName.Split(new[] {'.'});

            if (parts.Length > 0)
                return '.' + parts[0].ToUpper();

            return "_";
        }
    }
}
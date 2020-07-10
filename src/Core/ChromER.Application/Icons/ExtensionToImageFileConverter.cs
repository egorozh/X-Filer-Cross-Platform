using System;
using System.IO;
using System.Linq;

namespace ChromER.Application
{
    internal class ExtensionToImageFileConverter
    {
        public ExtensionToImageFileConverter()
        {
        }

        public FileInfo GetImagePath(string extension)
        {
            var applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var iconsDirectory = new DirectoryInfo(Path.Combine(applicationDirectory, "Resources", "Icons"));

            var icons = iconsDirectory
                .GetFiles()
                .ToDictionary(fi => GetNameWithoutExtension(fi.Name));

            if (icons.ContainsKey(extension.ToUpper()))
                return icons[extension.ToUpper()];

            return icons["._"];
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
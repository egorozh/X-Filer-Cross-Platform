using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChromER
{
    internal class ExtensionToImageFileConverter
    {
        #region Private Fields

        private readonly Dictionary<string, FileInfo> _icons;

        #endregion

        #region Constructor

        public ExtensionToImageFileConverter()
        {
            var applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var iconsDirectory = new DirectoryInfo(Path.Combine(applicationDirectory, "Resources", "Icons"));

            _icons = iconsDirectory
                .GetFiles()
                .ToDictionary(fi => GetNameWithoutExtension(fi.Name));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Получение полного пути до иконки для заданного расширения
        /// </summary>
        /// <param name="extension">Расширение в формате без точки сначала</param>
        /// <returns></returns>
        public FileInfo GetImagePath(string extension)
        {
            if (_icons.ContainsKey(extension.ToUpper()))
                return _icons[extension.ToUpper()];

            return _icons[IconName.Blank.ToUpper()];
        }

        #endregion

        #region Private Methods

        private static string GetNameWithoutExtension(string fileName)
        {
            // 7z.svg => 7z

            var parts = fileName.Split(new[] {'.'});

            if (parts.Length > 0)
                return parts[0].ToUpper();

            return "_";
        }

        #endregion
    }
}
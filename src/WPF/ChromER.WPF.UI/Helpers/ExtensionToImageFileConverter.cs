using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ChromER.WPF.UI
{
    internal static class ExtensionToImageFileConverter
    {
        private static readonly IconsSettings Settings;

        static ExtensionToImageFileConverter()  
        {
            Settings = IconsSettings.Open("icons.json");
        }

        public static FileInfo GetImagePath(string extension)
        {
            var applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (Settings.Icons.ContainsKey(extension))
            {
                var path = Settings.Icons[extension];
                return new FileInfo(Path.Combine(applicationDirectory, "Icons", path));
            }

            return new FileInfo(Path.Combine(applicationDirectory, "Icons", "041-3ds.svg"));
        }
    }

    public class IconsSettings
    {
        public Dictionary<string, string> Icons { get; set; } = new Dictionary<string, string>();

        public static IconsSettings Open(string path)
        {
            var json = File.ReadAllText(path);

            try
            {
                var settings = JsonSerializer.Deserialize<IconsSettings>(json);

                return settings;
            }
            catch (Exception e)
            {
            }

            return new IconsSettings();
        }

        public static void Save(IconsSettings settings, string path)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            try
            {
                var json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(path, json);
            }
            catch (Exception e)
            {
            }
        }
    }
}
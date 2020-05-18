using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Explorer.Shared.ViewModels;
using System;
using System.Globalization;

namespace Explorer.Avalonia.UI
{
    internal class FileEntityToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var drawingGroup = new DrawingGroup();

            if (value is FileEntityViewModel entityViewModel)
            {
                switch (entityViewModel)
                {
                    case DirectoryViewModel directoryViewModel:

                        return Application.Current.FindResource("FolderIconImage");

                    case FileViewModel fileViewModel:

                        return Application.Current.FindResource("FileIconImage");
                }
            }
            
            return drawingGroup;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
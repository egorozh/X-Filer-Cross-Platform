using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Egorozh.GoogleChromeWindow.WPF;

namespace ChromER.WPF.UI
{
    public class CalcMaxWidthTabItemConverter : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 &&
                values[0] is double controlWidth &&
                values[1] is int itemsCount)
            {
                var newWidth = (controlWidth - GoogleChromeWindow.SystemButtonsWidth - 10.0) / itemsCount;
                if (newWidth < 200)
                    return newWidth;
            }

            return 200.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
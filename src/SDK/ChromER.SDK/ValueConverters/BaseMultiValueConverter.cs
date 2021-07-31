using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace ChromER.SDK
{
    public abstract class BaseMultiValueConverter : MarkupExtension, IMultiValueConverter
    {
        public abstract object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture);

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
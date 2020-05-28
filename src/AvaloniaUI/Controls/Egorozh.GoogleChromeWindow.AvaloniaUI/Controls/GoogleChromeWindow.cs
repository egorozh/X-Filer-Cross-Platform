using Avalonia;
using Avalonia.Controls;

namespace Egorozh.GoogleChromeWindow.AvaloniaUI
{
    public class GoogleChromeWindow : Window
    {
        public static readonly StyledProperty<object> ToolBarContentProperty =
            AvaloniaProperty.Register<GoogleChromeWindow, object>(nameof(ToolBarContent));

        public object ToolBarContent
        {
            get => GetValue(ToolBarContentProperty);
            set => SetValue(ToolBarContentProperty, value);
        }

        public GoogleChromeWindow()
        {
        }
    }
}
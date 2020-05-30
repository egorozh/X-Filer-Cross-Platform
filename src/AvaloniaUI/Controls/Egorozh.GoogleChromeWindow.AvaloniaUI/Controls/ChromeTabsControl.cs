using System;
using Avalonia.Controls;
using Avalonia.Styling;

namespace Egorozh.GoogleChromeWindow.AvaloniaUI
{
    public class ChromeTabsControl : ListBox, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ChromeTabsControl);
    }
}
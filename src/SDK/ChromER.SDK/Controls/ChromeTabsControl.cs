using System;
using Avalonia.Controls;
using Avalonia.Styling;
using PropertyChanged;

namespace ChromER.SDK
{
    [DoNotNotify]
    public class ChromeTabsControl : ListBox, IStyleable
    {
        Type IStyleable.StyleKey => typeof(ChromeTabsControl);
    }
}
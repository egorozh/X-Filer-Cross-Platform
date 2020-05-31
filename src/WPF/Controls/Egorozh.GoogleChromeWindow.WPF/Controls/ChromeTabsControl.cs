using System.Windows;
using System.Windows.Controls;

namespace Egorozh.GoogleChromeWindow.WPF
{
    public class ChromeTabsControl : ListBox
    {
        #region Static Constructor

        static ChromeTabsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromeTabsControl),
                new FrameworkPropertyMetadata(typeof(ChromeTabsControl)));
        }

        #endregion
    }
}
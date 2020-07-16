using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Egorozh.GoogleChromeWindow.WPF
{
    public class ChromeTabsControl : ListBox
    {
        public static readonly DependencyProperty ActiveTabBackgroundProperty = DependencyProperty.Register(
            nameof(ActiveTabBackground), typeof(Brush), typeof(ChromeTabsControl),
            new PropertyMetadata(default(Brush)));

        public Brush ActiveTabBackground
        {
            get => (Brush) GetValue(ActiveTabBackgroundProperty);
            set => SetValue(ActiveTabBackgroundProperty, value);
        }

        #region Static Constructor

        static ChromeTabsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromeTabsControl),
                new FrameworkPropertyMetadata(typeof(ChromeTabsControl)));
        }

        #endregion
    }
}
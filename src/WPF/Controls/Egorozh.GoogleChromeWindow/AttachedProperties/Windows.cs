using System.Windows;

namespace Egorozh.GoogleChromeWindow
{
    internal class Windows
    {
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.RegisterAttached(
            "IsActive", typeof(bool), typeof(Windows), new PropertyMetadata(default(bool)));

        public static void SetIsActive(DependencyObject element, bool value)
        {
            element.SetValue(IsActiveProperty, value);
        }

        public static bool GetIsActive(DependencyObject element)
        {
            return (bool) element.GetValue(IsActiveProperty);
        }

        public static readonly DependencyProperty WindowStateProperty = DependencyProperty.RegisterAttached(
            "WindowState", typeof(WindowState), typeof(Windows), new PropertyMetadata(default(WindowState)));

        public static void SetWindowState(DependencyObject element, WindowState value)
        {
            element.SetValue(WindowStateProperty, value);
        }

        public static WindowState GetWindowState(DependencyObject element)
        {
            return (WindowState) element.GetValue(WindowStateProperty);
        }
    }
}
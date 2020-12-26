using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace Egorozh.GoogleChromeWindow.WPF
{
    internal class WindowResizeFixerBehavior : Behavior<Window>
    {
        private WindowResizer _fixer;

        protected override void OnAttached()
        {
            base.OnAttached();

            _fixer = new WindowResizer(AssociatedObject);

            AssociatedObject.StateChanged += (s, e) => UpdatePadding();

            AssociatedObject.Loaded += (s, e) => UpdatePadding();
        }

        private void UpdatePadding()
        {
            AssociatedObject.Padding = AssociatedObject.WindowState == WindowState.Maximized
                ? _fixer.CurrentMonitorMargin
                : new Thickness(0);
        }
    }
}
using System.Windows;
using Explorer.Shared.ViewModels;

namespace Explorer.WPF.UI
{
    public partial class MainWindow
    {
        private readonly MainViewModel _mainVm;

        public MainWindow()
        {
            InitializeComponent();

            _mainVm = new MainViewModel();

            DataContext = _mainVm;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainVm.ApplicationClosing();

            Application.Current.Shutdown();
        }

        private void ExpandButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
        }

        private void CollapseButton_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
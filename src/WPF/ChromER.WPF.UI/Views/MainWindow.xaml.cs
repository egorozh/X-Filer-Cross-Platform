using System.ComponentModel;
using System.Windows;
using Explorer.Shared.ViewModels;

namespace ChromER.WPF.UI
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

        protected override void OnClosing(CancelEventArgs e)
        {
            _mainVm.ApplicationClosing();

            Application.Current.Shutdown();
        }
    }
}
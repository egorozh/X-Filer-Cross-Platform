using Explorer.Shared.ViewModels;

namespace Explorer.WPF.UI
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}
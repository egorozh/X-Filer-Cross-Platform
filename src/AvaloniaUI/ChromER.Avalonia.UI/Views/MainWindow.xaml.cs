using Avalonia.Markup.Xaml;
using Egorozh.GoogleChromeWindow.AvaloniaUI;

namespace ChromER.Avalonia.UI
{
    public class MainWindow : GoogleChromeWindow
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            //this.AttachDevTools();
#endif

            _mainVm = ChromEr.Instance.MainViewModel;

            DataContext = _mainVm;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private readonly MainViewModel _mainVm;
    }
}
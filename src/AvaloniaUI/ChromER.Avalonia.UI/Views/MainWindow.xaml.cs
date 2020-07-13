using Avalonia.Markup.Xaml;
using ChromER.Shared.ViewModels;
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

            _mainVm = new MainViewModel(new AvaloniaSynchronizationHelper());

            DataContext = _mainVm;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private readonly MainViewModel _mainVm;
    }
}
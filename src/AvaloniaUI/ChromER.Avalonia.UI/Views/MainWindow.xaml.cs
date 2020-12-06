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

          

            
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
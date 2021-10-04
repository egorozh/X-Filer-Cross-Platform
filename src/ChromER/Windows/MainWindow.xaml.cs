using Avalonia;
using Avalonia.Markup.Xaml;
using ChromER.SDK;

namespace ChromER
{
    public class MainWindow : ChromerWindowBase
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ChromER
{
    public partial class TabView : UserControl
    {
        public TabView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

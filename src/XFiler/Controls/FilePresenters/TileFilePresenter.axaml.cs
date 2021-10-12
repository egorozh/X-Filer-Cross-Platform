using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ChromER
{
    public partial class TileFilePresenter : UserControl
    {
        public TileFilePresenter()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

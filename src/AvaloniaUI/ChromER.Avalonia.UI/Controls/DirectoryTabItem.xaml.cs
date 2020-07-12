using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ChromER.Avalonia.UI
{
    public class DirectoryTabItem : UserControl
    {
        public DirectoryTabItem()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
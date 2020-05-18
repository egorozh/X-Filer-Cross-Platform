using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Explorer.Avalonia.UI
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
using Avalonia.Markup.Xaml;

namespace ChromER
{
    public class App : ChromerApp
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
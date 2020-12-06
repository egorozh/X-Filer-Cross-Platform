using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace ChromER.Avalonia.UI
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            AvaloniaSynchronizationHelper shelper = new();

            ChromEr.CreateChromer(shelper, null);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = ChromEr.Instance.CreateMainViewModel(new []
                    {
                        new DirectoryTabItemViewModel(shelper)
                    })
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
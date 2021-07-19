using System;
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

            ChromEr.CreateChromer(shelper, null, null);
            AvaloniaSynchronizationHelper synchronizationHelper = new();
            var mainViewModel = ChromEr.Instance.CreateMainViewModel(Array.Empty<DirectoryTabItemViewModel>());

            var myCompTabVm = new DirectoryTabItemViewModel(synchronizationHelper, ChromEr.RootName, ChromEr.RootName);

            mainViewModel.TabItems.Add(myCompTabVm);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainViewModel
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
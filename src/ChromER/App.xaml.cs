using System.Collections.Generic;
using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ChromER.SDK;
using System.Globalization;
using System.IO;
using Dock.Model.Controls;
using Dock.Model.ReactiveUI.Controls;
using Dock.Serializer;

namespace ChromER
{
    public class App : Application, IChromerApp
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region Private Fields

        private ChromerTheme? _currentTheme;

        #endregion

        #region Public Properties

        public IContainer Host { get; }

        #endregion

        #region Constructor

        public App()
        {
            Host = new IoC().Build();

            CultureInfo currentCulture = new("Ru-ru");
            CultureInfo.CurrentCulture = currentCulture;
            CultureInfo.CurrentUICulture = currentCulture;
        }

        #endregion

        #region Public Methods

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var jsonLayout = string.Empty;

                if (File.Exists("layout.json"))
                    jsonLayout = File.ReadAllText("layout.json");

                var mainWindowViewModel = Host.Resolve<MainWindowViewModel>();
                var factory = Host.Resolve<MainDockFactory>();

                IRootDock layout;
                if (File.Exists("layout.json"))
                {
                    layout = new DockSerializer(typeof(List<>)).Deserialize<RootDock>(jsonLayout);
                    factory.InitLayoutAfterDeserialize(layout);
                }

                else
                {
                    layout = factory.CreateLayout();
                    factory.InitLayout(layout);
                }
                   

            

                mainWindowViewModel.Factory = factory;
                mainWindowViewModel.Layout = layout;

                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainWindowViewModel
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        #endregion

        #region Private Methods

        private void SetTheme(ChromerTheme newTheme)
        {
            if (_currentTheme != null)
            {
                //var resourceDictionaryToRemove =
                //    Resources.MergedDictionaries.FirstOrDefault(r => r.Source == _currentTheme.GetResourceUri());
                //if (resourceDictionaryToRemove != null)
                //    Resources.MergedDictionaries.Remove(resourceDictionaryToRemove);
            }

            _currentTheme = newTheme;

            //if (LoadComponent(_currentTheme.GetResourceUri()) is ResourceDictionary resourceDict)
            //    Resources.MergedDictionaries.Add(resourceDict);
        }

        #endregion
    }
}
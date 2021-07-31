using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ChromER.GoogleChromeStyle;
using ChromER.SDK;
using System.Globalization;

namespace ChromER
{
    public class ChromerApp : Application, IChromerApp
    {
        #region Private Fields

        private ChromerTheme? _currentTheme;

        #endregion

        #region Public Properties

        public IContainer Host { get; }

        public MainWindow MainWindow { get; private set; } = null!;

        #endregion

        #region Constructor

        public ChromerApp()
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
            SetTheme(new GoogleChromeTheme());

            ITabsFactory tabsFactory = Host.Resolve<ITabsFactory>();
            IExplorerTabFactory explorerTabFactory = Host.Resolve<IExplorerTabFactory>();

            var tabsViewModel = tabsFactory.CreateTabsViewModel(new[]
            {
                explorerTabFactory.CreateRootTab()
            });

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = tabsViewModel
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
using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ChromER.SDK;
using Dock.Model.Controls;
using Dock.Model.ReactiveUI.Controls;
using Dock.Serializer;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ChromER
{
    public class App : Application, IChromerApp
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        #region Private Fields
        
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

            this.DataContext = new AppModel();
        }

        #endregion

        #region Public Methods

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var factory = Host.Resolve<MainDockFactory>();

                const bool isLoadJson = false;

                IRootDock layout;

                if (isLoadJson && File.Exists("layout.json"))
                {
                    var jsonLayout =  File.ReadAllText("layout.json");

                    layout = new DockSerializer(typeof(List<>)).Deserialize<RootDock>(jsonLayout);
                    factory.InitLayoutAfterDeserialize(layout);
                }
                else
                {
                    layout = factory.CreateLayout();
                    factory.InitLayout(layout);
                }
                
                var mainWindowViewModel = Host.Resolve<MainWindowViewModel>();
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


        
        #endregion
    }

    public class AppModel : BaseViewModel
    {
    }
}
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using Point = System.Drawing.Point;

namespace ChromER.WPF.UI
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            WpfSynchronizationHelper synchronizationHelper = new();
            
            ChromEr.CreateChromer(synchronizationHelper, new BoundExampleInterTabClient(),
                ShowNewWindow);

            var mainViewModel = ChromEr.Instance.CreateMainViewModel(new DirectoryTabItemViewModel[0]);

            var myCompTabVm = new DirectoryTabItemViewModel(synchronizationHelper, ChromEr.RootName, ChromEr.RootName);

            mainViewModel.TabItems.Add(myCompTabVm);

            MainWindow mainWindow = new()
            {
                DataContext = mainViewModel
            };

            mainWindow.Show();

            //DebugWindow debugWindow = new();
            //debugWindow.Show();
        }

        private static void ShowNewWindow(TabsViewModel mvm, Point location)
        {
            var activeWindow = Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive) ?? Current.MainWindow;
            
            MainWindow mainWindow = new()
            {
                DataContext = mvm,
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = activeWindow.Left + location.X,
                Top = activeWindow.Top + location.Y
            };

            mainWindow.Show();
        }
    }
}
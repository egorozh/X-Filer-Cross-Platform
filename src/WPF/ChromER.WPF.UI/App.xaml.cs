using System.Globalization;
using System.Windows;
using System.Windows.Markup;

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

            ChromEr.CreateChromer(synchronizationHelper, () => new BoundExampleInterTabClient());

            MainWindow mainWindow = new()
            {
                DataContext =
                    ChromEr.Instance.CreateMainViewModel(new DirectoryTabItemViewModel[] {new(synchronizationHelper),})
            };

            mainWindow.Show();

            //DebugWindow debugWindow = new();
            //debugWindow.Show();
        }
    }
}
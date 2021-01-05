using System.Windows;

namespace ChromER.WPF.UI
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

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
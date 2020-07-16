using System.Windows;

namespace ChromER.WPF.UI
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ChromEr.CreateChromer(new WpfSynchronizationHelper());

            var mainWindow = new MainWindow();

            mainWindow.Show();
        }
    }
}
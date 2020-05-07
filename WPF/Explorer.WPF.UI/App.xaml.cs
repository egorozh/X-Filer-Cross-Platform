using System.Windows;

namespace Explorer.WPF.UI
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();

            mainWindow.Show();
        }
    }
}
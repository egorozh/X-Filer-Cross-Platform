using System;
using System.Collections.Generic;
using System.Windows;
using Dragablz;
using Dragablz.Dockablz;

namespace ChromER.WPF.UI
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            WpfSynchronizationHelper synchronizationHelper = new ();

            ChromEr.CreateChromer(synchronizationHelper, () => new BoundExampleInterTabClient());

            MainWindow mainWindow = new ()
            {
                DataContext =
                    ChromEr.Instance.CreateMainViewModel(new DirectoryTabItemViewModel[] {new (synchronizationHelper),})
            };

            mainWindow.Show();
        }
    }

    public class BoundExampleInterTabClient : IInterTabClient, ITabClient
    {
        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            MainWindow view = new ();
            var model = ChromEr.Instance.CreateMainViewModel(new List<DirectoryTabItemViewModel>());
            view.DataContext = model;
            return new NewTabHost<Window>(view, view.InitialTabablzControl);
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            return TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }

    public static class CloseActionStorage
    {
        public static ItemActionCallback ClosingTabItemHandler => ClosingTabItemHandlerImpl;

        private static void ClosingTabItemHandlerImpl(ItemActionCallbackArgs<TabablzControl> args)
        {
            //in here you can dispose stuff or cancel the close

            //here's your view model:
            var viewModel = args.DragablzItem.DataContext as DirectoryTabItemViewModel;

            //here's how you can cancel stuff:
            //args.Cancel(); 
        }

        public static ClosingFloatingItemCallback ClosingFloatingItemHandler => ClosingFloatingItemHandlerImpl;

        /// <summary>
        /// Callback to handle floating toolbar/MDI window closing.
        /// </summary>        
        private static void ClosingFloatingItemHandlerImpl(ItemActionCallbackArgs<Layout> args)
        {
            //in here you can dispose stuff or cancel the close

            //here's your view model: 
            if (args.DragablzItem.DataContext is IDisposable disposable)
                disposable.Dispose();

            //here's how you can cancel stuff:
            //args.Cancel(); 
        }
    }
}
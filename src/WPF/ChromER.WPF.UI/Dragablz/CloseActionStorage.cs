using Dragablz;

namespace ChromER.WPF.UI
{
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
    }
}
using System.Collections.Generic;
using System.Windows;
using Dragablz;

namespace ChromER.WPF.UI
{
    public class BoundExampleInterTabClient : IInterTabClient, ITabClient
    {
        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            MainWindow view = new();
            var model = ChromEr.Instance.CreateMainViewModel(new List<DirectoryTabItemViewModel>());
            view.DataContext = model;
            return new NewTabHost<Window>(view, view.InitialTabablzControl);
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            return TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }
}
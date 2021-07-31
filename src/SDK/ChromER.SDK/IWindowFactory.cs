using Prism.Commands;

namespace ChromER.SDK
{
    public interface IWindowFactory
    {
        DelegateCommand<FileEntityViewModel> OpenNewWindowCommand { get; }
            
        void OpenTabInNewWindow(IExplorerTabItemViewModel tabItem);
    }
}
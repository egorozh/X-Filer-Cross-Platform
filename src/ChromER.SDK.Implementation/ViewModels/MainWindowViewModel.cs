using Dock.Model.Core;
using Dock.Model.ReactiveUI.Controls;

namespace ChromER.SDK
{
    public class MainWindowViewModel : BaseViewModel
    {
        public IFactory Factory { get; set; }
        public IDock Layout { get; set; }
    }

    internal class MainViewModel : RootDock
    {
    }
}
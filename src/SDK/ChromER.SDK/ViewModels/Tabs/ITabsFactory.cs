using System.Collections.Generic;

namespace ChromER.SDK
{
    public interface ITabsFactory
    {
        ITabsViewModel CreateTabsViewModel(IEnumerable<ITabItem> initItems);
        ITabsViewModel CreateTabsViewModel();   
    }
}
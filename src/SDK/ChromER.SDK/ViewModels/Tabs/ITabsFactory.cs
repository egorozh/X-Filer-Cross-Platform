using System.Collections.Generic;
using Dock.Model.Controls;

namespace ChromER.SDK
{
    public interface ITabsFactory
    {
        ITabsViewModel CreateTabsViewModel(IEnumerable<IDocument> initItems);
        ITabsViewModel CreateTabsViewModel();   
    }
}
using System.Collections.Generic;
using System.Linq;
using Dock.Model.Controls;

namespace ChromER.SDK
{
    public class TabsFactory : ITabsFactory
    {
        private readonly IExplorerTabFactory _explorerTabFactory;
        private readonly IWindowFactory _windowFactory;
        private readonly IBookmarksManager _bookmarksManager;

        public TabsFactory(IExplorerTabFactory explorerTabFactory,
            IWindowFactory windowFactory, IBookmarksManager bookmarksManager)
        {
            _explorerTabFactory = explorerTabFactory;
            _windowFactory = windowFactory;
            _bookmarksManager = bookmarksManager;
        }
    
        public ITabsViewModel CreateTabsViewModel(IEnumerable<IDocument> initItems)
            => new TabsViewModel( _explorerTabFactory, _windowFactory, _bookmarksManager, initItems);

        public ITabsViewModel CreateTabsViewModel()
            => new TabsViewModel( _explorerTabFactory, _windowFactory, _bookmarksManager,
                Enumerable.Empty<IDocument>());
    }
}
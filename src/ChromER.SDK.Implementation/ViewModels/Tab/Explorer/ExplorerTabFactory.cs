using System.IO;
using Dock.Model.Controls;

namespace ChromER.SDK
{
    public class ExplorerTabFactory : IExplorerTabFactory
    {
        private readonly IFilesPresenterFactory _filesPresenterFactory;
        private readonly IBookmarksManager _bookmarksManager;

        public ExplorerTabFactory(IFilesPresenterFactory filesPresenterFactory,
            IBookmarksManager bookmarksManager)
        {
            _filesPresenterFactory = filesPresenterFactory;
            _bookmarksManager = bookmarksManager;
        }

        public IDocument CreateExplorerTab(DirectoryInfo directoryInfo)
            => CreateExplorerTabItemViewModel(directoryInfo);

        public IDocument CreateExplorerTab(string dirPath, string name)
            => CreateExplorerTabItemViewModel(dirPath, name);

        public IDocument CreateRootTab()
            => CreateExplorerTabItemViewModel(IChromerApp.RootName, IChromerApp.RootName);

        private ExplorerTabItemViewModel CreateExplorerTabItemViewModel(DirectoryInfo directoryInfo)
        {
            var item = new ExplorerTabItemViewModel();

            item.Init(_filesPresenterFactory, _bookmarksManager, directoryInfo);

            return item;
        }

        private ExplorerTabItemViewModel CreateExplorerTabItemViewModel(string dirPath, string name)
        {
            var item = new ExplorerTabItemViewModel();

            item.Init(_filesPresenterFactory, _bookmarksManager, dirPath, name);

            return item;
        }
    }
}
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Dock.Model.Controls;

namespace ChromER.SDK
{
    public class TabsViewModel : BaseViewModel, ITabsViewModel
    {
        #region Private Fields

        private readonly IExplorerTabFactory _explorerTabFactory;
        private readonly IWindowFactory _windowFactory;

        #endregion

        #region Public Properties

        public ObservableCollection<IDocument> TabItems { get; }

        public IDocument? CurrentTabItem { get; set; }

        public IReadOnlyCollection<IMenuItemViewModel> Bookmarks { get; }

        public Func<IDocument> Factory { get; }
            
        #endregion

        #region Commands

        public DelegateCommand<object> CreateNewTabItemCommand { get; }
        public DelegateCommand<object> OpenTabItemInNewWindowCommand { get; }
        public DelegateCommand<object> DuplicateTabCommand { get; }
        public DelegateCommand<object> CloseAllTabsCommand { get; }
        public DelegateCommand<IDocument> CloseTabCommand { get; }

        #endregion

        #region Constructor

        public TabsViewModel(IExplorerTabFactory explorerTabFactory,
            IWindowFactory windowFactory,
            IBookmarksManager bookmarksManager,
            IEnumerable<IDocument> init)
        {
            _explorerTabFactory = explorerTabFactory;
            _windowFactory = windowFactory;

            Bookmarks = bookmarksManager.Bookmarks;

            CreateNewTabItemCommand = new DelegateCommand<object>(OnCreateNewTabItem);
            OpenTabItemInNewWindowCommand =
                new DelegateCommand<object>(OnOpenTabItemInNewWindow, OnCanOpenTabItemInNewWindow);
            DuplicateTabCommand = new DelegateCommand<object>(OnDuplicate);
            CloseAllTabsCommand = new DelegateCommand<object>(OnCloseAllTabs, CanCloseAllTabs);

            CloseTabCommand = new DelegateCommand<IDocument>(OnCloseTab);

            TabItems = new ObservableCollection<IDocument>(init);
            CurrentTabItem = TabItems.FirstOrDefault();

            Factory = CreateTabVm;

            TabItems.CollectionChanged += TabItemsOnCollectionChanged;
        }

        #endregion

        #region Public Methods

        public void OnOpenNewTab(FileEntityViewModel fileEntityViewModel, bool isSelectNewTab = false)
        {
            if (fileEntityViewModel is DirectoryViewModel directoryViewModel)
            {
                var tab = _explorerTabFactory.CreateExplorerTab(directoryViewModel.DirectoryInfo);
                TabItems.Add(tab);

                if (isSelectNewTab)
                    CurrentTabItem = tab;
            }
        }

        private void OnCreateNewTabItem(object? obj)
        {
            TabItems.Add(_explorerTabFactory.CreateRootTab());
        }

        private bool OnCanOpenTabItemInNewWindow(object? obj) => TabItems.Count > 1;

        private void OnOpenTabItemInNewWindow(object? obj)
        {
            if (obj is not ExplorerTabItemViewModel directoryTabItem)
                return;

            //TabItems.Remove(directoryTabItem);

            //_windowFactory.OpenTabInNewWindow(directoryTabItem);
        }

        private void OnDuplicate(object? obj)
        {
            if (obj is not ExplorerTabItemViewModel directoryTabItem)
                return;

            TabItems.Add(_explorerTabFactory
                .CreateExplorerTab(
                    directoryTabItem.CurrentDirectoryFileName,
                    directoryTabItem.Title));
        }

        private bool CanCloseAllTabs(object? obj) => TabItems.Count > 1;

        private void OnCloseAllTabs(object? obj)
        {
            if (obj is not ExplorerTabItemViewModel directoryTabItem)
                return;

            var removedItems = TabItems.Where(i => i != directoryTabItem).ToList();

            foreach (var item in removedItems)
                TabItems.Remove(item);
        }

        private void OnCloseTab(IDocument tab) => TabItems.Remove(tab);

        #endregion

        #region Private Methods

        private IDocument CreateTabVm() => _explorerTabFactory.CreateRootTab();

        private void TabItemsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OpenTabItemInNewWindowCommand.RaiseCanExecuteChanged();
            CloseAllTabsCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
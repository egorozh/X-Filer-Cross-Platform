using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChromER
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ISynchronizationHelper _synchronizationHelper;

        #region Public Properties

        public ITabClient InterTabClient { get; }

        public ObservableCollection<DirectoryTabItemViewModel> DirectoryTabItems { get; }

        public ObservableCollection<DirectoryTabItemViewModel> ToolItems { get; set; } = new();

        public DirectoryTabItemViewModel CurrentDirectoryTabItem { get; set; }

        public IReadOnlyCollection<MenuItemViewModel> Bookmarks => ChromEr.Instance.BookmarksManager.Bookmarks;

        public Func<DirectoryTabItemViewModel> Factory { get; }

        #endregion
        
        #region Constructor

        public MainViewModel(ISynchronizationHelper synchronizationHelper, ITabClient tabClient,
            IEnumerable<DirectoryTabItemViewModel> init)
        {
            _synchronizationHelper = synchronizationHelper;
            InterTabClient = tabClient;

            DirectoryTabItems = new ObservableCollection<DirectoryTabItemViewModel>(init);

            Factory = CreateTabVm;
        }

        #endregion

        #region Private Methods

        private DirectoryTabItemViewModel CreateTabVm() => new(_synchronizationHelper);

        #endregion
    }

    /// <summary>
    /// View-Model MenuItem'а 
    /// </summary>
    public class MenuItemViewModel : BaseViewModel
    {
        public string Path { get; set; }

        public string Header { get; set; }

        public ICommand Command { get; set; }

        public object CommandParameter { get; set; }

        public string IconPath { get; set; }

        public IList<MenuItemViewModel> Items { get; set; }

        public MenuItemViewModel(string path)
        {
            Path = path;

            CommandParameter = path;
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ChromER.Shared.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ISynchronizationHelper _synchronizationHelper;

        #region Public Properties

        public ObservableCollection<DirectoryTabItemViewModel> DirectoryTabItems { get; set; } =
            new ObservableCollection<DirectoryTabItemViewModel>();

        public DirectoryTabItemViewModel CurrentDirectoryTabItem { get; set; }

        public ObservableCollection<MenuItemViewModel> Bookmarks { get; private set; }

        #endregion

        #region Commands

        public DelegateCommand AddTabItemCommand { get; }

        public DelegateCommand CloseCommand { get; }

        public DelegateCommand BookmarkClickCommand { get; }

        #endregion

        #region Events

        #endregion

        #region Constructor

        public MainViewModel(ISynchronizationHelper synchronizationHelper)
        {
            _synchronizationHelper = synchronizationHelper;
            AddTabItemCommand = new DelegateCommand(OnAddTabItem);
            CloseCommand = new DelegateCommand(OnClose);

            AddTabItemViewModel();

            BookmarkClickCommand = new DelegateCommand(OnBookmarkClicked);

            Bookmarks = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel("C:\\")
                {
                    Header = "C:\\",
                    Command = BookmarkClickCommand
                },
                new MenuItemViewModel(@"C:\Games\ArtMoney")
                {
                    Header = @"ArtMoney",
                    Command = BookmarkClickCommand
                }
            };
        }

        #endregion

        #region Public Methods

        public void ApplicationClosing()
        {
        }

        #endregion

        #region Commands Methods

        private void OnBookmarkClicked(object parameter)
        {
            if (parameter is string path)
                CurrentDirectoryTabItem.OpenBookmark(path);
        }

        private void OnAddTabItem(object obj)
        {
            AddTabItemViewModel();
        }

        private void OnClose(object obj)
        {
            if (obj is DirectoryTabItemViewModel directoryTabItemViewModel)
            {
                CloseTab(directoryTabItemViewModel);
            }
        }

        #endregion

        #region Private Methods

        private void AddTabItemViewModel()
        {
            var vm = new DirectoryTabItemViewModel(_synchronizationHelper);

            DirectoryTabItems.Add(vm);
            CurrentDirectoryTabItem = vm;
        }

        private void CloseTab(DirectoryTabItemViewModel directoryTabItemViewModel)
        {
            DirectoryTabItems.Remove(directoryTabItemViewModel);

            CurrentDirectoryTabItem = DirectoryTabItems.FirstOrDefault();
        }

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

        public IList<MenuItemViewModel> Items { get; set; }

        public MenuItemViewModel(string path)
        {
            Path = path;

            CommandParameter = path;
        }
    }
}
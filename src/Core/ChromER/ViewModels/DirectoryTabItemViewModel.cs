using System;
using System.Diagnostics;
using System.IO;

namespace ChromER
{
    public class DirectoryTabItemViewModel : ChromerTabItemViewModel
    {
        #region Private Fields

        private readonly IDirectoryHistory _history;

        private readonly ISynchronizationHelper _synchronizationHelper;
        private string _searchText;
        private bool _isTilePresenter;
        private bool _isGridPresenter;

        #endregion

        #region Public Properties

        public bool IsTilePresenter
        {
            get => _isTilePresenter;
            set
            {
                _isTilePresenter = value;

                if (value)
                {
                    IsGridPresenter = false;
                    OpenDirectory();
                }
                else if (!_isGridPresenter)
                {
                    IsTilePresenter = true;
                }
            }
        }

        public bool IsGridPresenter
        {
            get => _isGridPresenter;
            set
            {
                _isGridPresenter = value;

                if (value)
                {
                    IsTilePresenter = false;
                    OpenDirectory();
                }
                else if (!_isTilePresenter)
                {
                    IsGridPresenter = true;
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set => SetSearchText(value);
        }

        public string CurrentDirectoryFileName => _history.Current.DirectoryPath;

        public IFilesPresenter? FilesPresenter { get; set; }

        #endregion

        #region Commands

        public DelegateCommand AddBookmarkCommand => ChromEr.Instance.BookmarksManager.AddBookmarkCommand;

        public DelegateCommand MoveBackCommand { get; }

        public DelegateCommand MoveForwardCommand { get; }

        #endregion

        #region Constructor

        public DirectoryTabItemViewModel(ISynchronizationHelper synchronizationHelper)
        {
            _synchronizationHelper = synchronizationHelper;

            _history = new DirectoryHistory(ChromEr.RootName, ChromEr.RootName);

            MoveBackCommand = new DelegateCommand(OnMoveBack, OnCanMoveBack);
            MoveForwardCommand = new DelegateCommand(OnMoveForward, OnCanMoveForward);

            Header = _history.Current.DirectoryPathName;
            SearchText = _history.Current.DirectoryPath;

            IsTilePresenter = true;

            _history.HistoryChanged += History_HistoryChanged;
        }

        #endregion

        #region Public Methods

        public void OpenBookmark(string path)
        {
            var attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
                Open(new DirectoryViewModel(new DirectoryInfo(path)));
            else
                Open(new FileViewModel(new FileInfo(path)));
        }

        #endregion

        #region Commands Methods

        private void Open(FileEntityViewModel parameter)
        {
            if (parameter is DirectoryViewModel directoryViewModel)
            {
                SearchText = directoryViewModel.FullName;
                Header = directoryViewModel.Name;

                _history.Add(SearchText, Header);

                OpenDirectory();
            }
            else if (parameter is FileViewModel fileViewModel)
            {
                new Process
                {
                    StartInfo = new ProcessStartInfo(fileViewModel.FullName)
                    {
                        UseShellExecute = true
                    }
                }.Start();
            }
        }

        private bool OnCanMoveForward(object obj) => _history.CanMoveForward;

        private void OnMoveForward(object obj)
        {
            _history.MoveForward();

            var current = _history.Current;

            SearchText = current.DirectoryPath;
            Header = current.DirectoryPathName;

            OpenDirectory();
        }

        private bool OnCanMoveBack(object obj) => _history.CanMoveBack;

        private void OnMoveBack(object obj)
        {
            _history.MoveBack();

            var current = _history.Current;

            SearchText = current.DirectoryPath;
            Header = current.DirectoryPathName;

            OpenDirectory();
        }

        #endregion

        #region Private Methods

        private void OpenDirectory()
        {
            if (FilesPresenter != null)
            {
                FilesPresenter.DirectoryOrFileOpened -= FilePresenterOnDirectoryOrFileOpened;
                FilesPresenter.Dispose();
            }

            FilesPresenter = _isGridPresenter
                ? new GridFilesPresenterViewModel(_synchronizationHelper, CurrentDirectoryFileName)
                : new TileFilesPresenterViewModel(_synchronizationHelper, CurrentDirectoryFileName);

            FilesPresenter.DirectoryOrFileOpened += FilePresenterOnDirectoryOrFileOpened;
        }

        private void FilePresenterOnDirectoryOrFileOpened(object? sender, OpenDirectoryEventArgs e)
        {
            Open(e.FileEntityViewModel);
        }

        private void History_HistoryChanged(object? sender, EventArgs e)
        {
            MoveBackCommand?.RaiseCanExecuteChanged();
            MoveForwardCommand?.RaiseCanExecuteChanged();
        }

        private void SetSearchText(string text)
        {
            _searchText = text;
        }

        #endregion
    }
}
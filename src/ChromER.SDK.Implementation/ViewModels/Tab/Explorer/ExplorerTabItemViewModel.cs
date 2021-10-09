﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Dock.Model.ReactiveUI.Controls;
using Prism.Commands;

namespace ChromER.SDK
{
    public class ExplorerTabItemViewModel : Document
    {
        #region Private Fields

        private IDirectoryHistory _history;
        private IFilesPresenterFactory _filesPresenterFactory;
        private string _searchText;

        #endregion

        #region Public Properties

        public bool IsTilePresenter { get; set; }

        public bool IsGridPresenter { get; set; }

        public string SearchText
        {
            get => _searchText;
            set => SetSearchText(value);
        }

        public string CurrentDirectoryFileName => _history.Current.DirectoryPath;

        public IFilesPresenter? FilesPresenter { get; set; }

        #endregion

        #region Commands

        public DelegateCommand<string> AddBookmarkCommand { get; private set; }

        public DelegateCommand MoveBackCommand { get; private set; }

        public DelegateCommand MoveForwardCommand { get; private set; }

        #endregion

        #region Constructor

        public void Init(
            IFilesPresenterFactory filesPresenterFactory,
            IBookmarksManager bookmarksManager,
            string directoryPath,
            string directoryName) 
        {
            _filesPresenterFactory = filesPresenterFactory;
            AddBookmarkCommand = bookmarksManager.AddBookmarkCommand;

            _history = new DirectoryHistory(directoryPath, directoryName);

            MoveBackCommand = new DelegateCommand(OnMoveBack, OnCanMoveBack);
            MoveForwardCommand = new DelegateCommand(OnMoveForward, OnCanMoveForward);

            Title = _history.Current.DirectoryPathName;
            _searchText = _history.Current.DirectoryPath;

            _history.HistoryChanged += History_HistoryChanged;

            PropertyChanged += DirectoryTabItemViewModelOnPropertyChanged;

            IsTilePresenter = true;
        }

        public void Init(IFilesPresenterFactory filesPresenterFactory,
            IBookmarksManager bookmarksManager,
            DirectoryInfo directoryInfo)
        {
            Init(filesPresenterFactory, bookmarksManager, directoryInfo.FullName, directoryInfo.Name);
        }

        private void DirectoryTabItemViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChanged -= DirectoryTabItemViewModelOnPropertyChanged;

            switch (e.PropertyName)
            {
                case nameof(IsTilePresenter):

                    if (IsTilePresenter)
                    {
                        IsGridPresenter = false;
                        OpenDirectory();
                    }
                    else if (!IsGridPresenter)
                    {
                        IsTilePresenter = true;
                    }

                    break;
                case nameof(IsGridPresenter):

                    if (IsGridPresenter)
                    {
                        IsTilePresenter = false;
                        OpenDirectory();
                    }
                    else if (!IsTilePresenter)
                    {
                        IsGridPresenter = true;
                    }

                    break;
            }

            PropertyChanged += DirectoryTabItemViewModelOnPropertyChanged;
        }

        #endregion

        #region Public Methods

        public void OpenBookmark(string path)
        {
            var attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
                OpenDirectory(new DirectoryInfo(path));
            else
                OpenFile(path);
        }

        #endregion

        #region Commands Methods

        private void Open(FileEntityViewModel parameter)
        {
            switch (parameter)
            {
                case DirectoryViewModel directoryViewModel:
                    OpenDirectory(directoryViewModel.DirectoryInfo);
                    break;
                case FileViewModel fileViewModel:
                    OpenFile(fileViewModel.FullName);
                    break;
            }
        }


        private bool OnCanMoveForward() => _history.CanMoveForward;

        private void OnMoveForward()
        {
            _history.MoveForward();

            var current = _history.Current;

            SearchText = current.DirectoryPath;
            Title = current.DirectoryPathName;

            OpenDirectory();
        }

        private bool OnCanMoveBack() => _history.CanMoveBack;

        private void OnMoveBack()
        {
            _history.MoveBack();

            var current = _history.Current;

            SearchText = current.DirectoryPath;
            Title = current.DirectoryPathName;

            OpenDirectory();
        }

        #endregion

        #region Private Methods

        private void OpenDirectory(DirectoryInfo directoryInfo)
        {
            SearchText = directoryInfo.FullName;
            Title = directoryInfo.Name;

            _history.Add(SearchText, Title);

            OpenDirectory();
        }

        private static void OpenFile(string path) => new Process
        {
            StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            }
        }.Start();

        private void OpenDirectory()
        {
            if (FilesPresenter != null)
            {
                FilesPresenter.DirectoryOrFileOpened -= FilePresenterOnDirectoryOrFileOpened;
                FilesPresenter.Dispose();
            }

            var presenterType = IsGridPresenter ? PresenterType.Grid : PresenterType.RegularTile;

            FilesPresenter = _filesPresenterFactory.CreatePresenter(presenterType, CurrentDirectoryFileName);

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
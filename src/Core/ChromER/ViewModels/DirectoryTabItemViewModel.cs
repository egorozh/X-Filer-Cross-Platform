using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChromER
{
    public class DirectoryTabItemViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly IDirectoryHistory _history;
        private readonly BackgroundWorker _backgroundWorker;
        private readonly ISynchronizationHelper _synchronizationHelper;

        #endregion

        #region Public Properties

        public string FilePath { get; set; }

        public string Header { get; set; }

        public bool IsSelected { get; set; }

        public object Content { get; set; }

        public ObservableCollection<FileEntityViewModel> DirectoriesAndFiles { get; set; } = new();

        public FileEntityViewModel SelectedFileEntity { get; set; }

        #endregion

        #region Commands

        public DelegateCommand AddBookmarkCommand => ChromEr.Instance.BookmarksManager.AddBookmarkCommand;

        public DelegateCommand OpenCommand { get; }

        public DelegateCommand MoveBackCommand { get; }

        public DelegateCommand MoveForwardCommand { get; }

        #endregion

        #region Constructor

        public DirectoryTabItemViewModel(ISynchronizationHelper synchronizationHelper)
        {
            _synchronizationHelper = synchronizationHelper;

            _backgroundWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };

            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;

            _history = new DirectoryHistory("Мой компьютер", "Мой компьютер");

            OpenCommand = new DelegateCommand(Open);
            MoveBackCommand = new DelegateCommand(OnMoveBack, OnCanMoveBack);
            MoveForwardCommand = new DelegateCommand(OnMoveForward, OnCanMoveForward);

            Header = _history.Current.DirectoryPathName;
            FilePath = _history.Current.DirectoryPath;

            OpenDirectory();

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

        private void Open(object parameter)
        {
            if (parameter is DirectoryViewModel directoryViewModel)
            {
                FilePath = directoryViewModel.FullName;
                Header = directoryViewModel.Name;

                _history.Add(FilePath, Header);

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

            FilePath = current.DirectoryPath;
            Header = current.DirectoryPathName;

            OpenDirectory();
        }

        private bool OnCanMoveBack(object obj) => _history.CanMoveBack;

        private void OnMoveBack(object obj)
        {
            _history.MoveBack();

            var current = _history.Current;

            FilePath = current.DirectoryPath;
            Header = current.DirectoryPathName;

            OpenDirectory();
        }

        #endregion

        #region Private Methods

        private void OpenDirectory()
        {
            if (_backgroundWorker.IsBusy)
                _backgroundWorker.CancelAsync();
            else
                RunWorker();
        }

        private void RunWorker()
        {
            DirectoriesAndFiles.Clear();

            if (Header == "Мой компьютер")
            {
                foreach (var logicalDrive in Directory.GetLogicalDrives())
                    DirectoriesAndFiles.Add(new LogicalDriveViewModel(logicalDrive));

                return;
            }

            var directoryInfo = new DirectoryInfo(FilePath);

            _backgroundWorker.RunWorkerAsync(directoryInfo);
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                RunWorker();
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var directoryInfo = e.Argument as DirectoryInfo;

            var bw = sender as BackgroundWorker;

            try
            {
                var directories = directoryInfo.EnumerateDirectories()
                    .OrderBy(d => d.Name, new NaturalSortComparer());

                //var allCount = directories.Count();

                foreach (var directory in directories)
                {
                    if (bw.CancellationPending)
                    {
                        e.Cancel = true;

                        return;
                    }

                    _synchronizationHelper.InvokeAsync(() =>
                    {
                        DirectoriesAndFiles.Add(new DirectoryViewModel(directory));
                    }).Wait();
                }

                foreach (var fileInfo in directoryInfo.GetFiles())
                {
                    if (bw.CancellationPending)
                    {
                        e.Cancel = true;

                        return;
                    }

                    _synchronizationHelper.InvokeAsync(() => { DirectoriesAndFiles.Add(new FileViewModel(fileInfo)); })
                        .Wait();
                }
            }
            catch (Exception ex)
            {
                //TODO: Try Exception 
            }
        }

        private void History_HistoryChanged(object sender, EventArgs e)
        {
            MoveBackCommand?.RaiseCanExecuteChanged();
            MoveForwardCommand?.RaiseCanExecuteChanged();
        }

        #endregion
    }

    public interface ISynchronizationHelper
    {
        Task InvokeAsync(Action action);
    }

    public class NaturalSortComparer : IComparer<string>, IDisposable
    {
        #region Private Fields

        private readonly bool _isAscending;

        private Dictionary<string, string[]> _table = new();

        #endregion
        
        #region Constructor

        public NaturalSortComparer(bool inAscendingOrder = true)
        {
            this._isAscending = inAscendingOrder;
        }

        #endregion
        
        #region Public Methods

        public int Compare(string x, string y)
        {
            if (x == y)
                return 0;

            if (!_table.TryGetValue(x, out var x1))
            {
                x1 = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
                _table.Add(x, x1);
            }

            if (!_table.TryGetValue(y, out var y1))
            {
                y1 = Regex.Split(y.Replace(" ", ""), "([0-9]+)");
                _table.Add(y, y1);
            }

            int returnVal;

            for (int i = 0; i < x1.Length && i < y1.Length; i++)
            {
                if (x1[i] != y1[i])
                {
                    returnVal = PartCompare(x1[i], y1[i]);
                    return _isAscending ? returnVal : -returnVal;
                }
            }

            if (y1.Length > x1.Length)
            {
                returnVal = 1;
            }
            else if (x1.Length > y1.Length)
            {
                returnVal = -1;
            }
            else
            {
                returnVal = 0;
            }

            return _isAscending ? returnVal : -returnVal;
        }

        public void Dispose()
        {
            _table.Clear();
            _table = null;
        }

        #endregion

        #region Private Methods

        private static int PartCompare(string left, string right)
        {
            int x, y;
            if (!int.TryParse(left, out x))
                return left.CompareTo(right);

            if (!int.TryParse(right, out y))
                return left.CompareTo(right);

            return x.CompareTo(y);
        }

        #endregion
    }
}
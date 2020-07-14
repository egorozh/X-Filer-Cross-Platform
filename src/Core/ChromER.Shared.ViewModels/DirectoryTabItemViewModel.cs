using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ChromER.Shared.ViewModels
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

        public string Name { get; set; }

        public ObservableCollection<FileEntityViewModel> DirectoriesAndFiles { get; set; } =
            new ObservableCollection<FileEntityViewModel>();

        public FileEntityViewModel SelectedFileEntity { get; set; }

        #endregion

        #region Commands

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

            Name = _history.Current.DirectoryPathName;
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
                Name = directoryViewModel.Name;

                _history.Add(FilePath, Name);

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
            Name = current.DirectoryPathName;

            OpenDirectory();
        }

        private bool OnCanMoveBack(object obj) => _history.CanMoveBack;

        private void OnMoveBack(object obj)
        {
            _history.MoveBack();

            var current = _history.Current;

            FilePath = current.DirectoryPath;
            Name = current.DirectoryPathName;

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

            if (Name == "Мой компьютер")
            {
                foreach (var logicalDrive in Directory.GetLogicalDrives())
                    DirectoriesAndFiles.Add(new DirectoryViewModel(logicalDrive));

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
                var directories = directoryInfo.EnumerateDirectories();

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
}
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace ChromER
{
    public abstract class BaseFilesPresenter : BaseViewModel, IFilesPresenter
    {
        #region Private Fields

        private readonly ISynchronizationHelper _synchronizationHelper;
        private readonly BackgroundWorker _backgroundWorker;

        #endregion

        #region Public Properties

        public ObservableCollection<FileEntityViewModel> DirectoriesAndFiles { get; set; } = new();

        public FileEntityViewModel? SelectedFileEntity { get; set; }

        public string CurrentDirectoryPathName { get; }

        #endregion

        #region Events
        
        public event EventHandler<OpenDirectoryEventArgs>? DirectoryOrFileOpened;

        #endregion

        #region Commands

        public DelegateCommand OpenCommand { get; }

        #endregion

        #region Constructor

        protected BaseFilesPresenter(ISynchronizationHelper synchronizationHelper, string directoryPathName)
        {
            _synchronizationHelper = synchronizationHelper;
            CurrentDirectoryPathName = directoryPathName;

            _backgroundWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };

            OpenCommand = new DelegateCommand(Open);

            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;

            _backgroundWorker.RunWorkerAsync();
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            if (_backgroundWorker.IsBusy)
                _backgroundWorker.CancelAsync();

            _backgroundWorker.RunWorkerCompleted -= BackgroundWorker_RunWorkerCompleted;
            _backgroundWorker.DoWork -= BackgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged -= BackgroundWorker_ProgressChanged;

            _backgroundWorker?.Dispose();
        }

        #endregion

        #region Command Methods

        private void Open(object parameter)
        {
            if (parameter is FileEntityViewModel fileEntityViewModel)
                DirectoryOrFileOpened?.Invoke(this, new OpenDirectoryEventArgs(fileEntityViewModel));
        }

        #endregion

        #region Private Methods

        private void BackgroundWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                _backgroundWorker.RunWorkerAsync();
            }
        }

        private void BackgroundWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
        }

        private async void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (CurrentDirectoryPathName == ChromEr.RootName)
            {
                foreach (var logicalDrive in Directory.GetLogicalDrives())
                {
                    await _synchronizationHelper.InvokeAsync(() =>
                    {
                        DirectoriesAndFiles.Add(new LogicalDriveViewModel(logicalDrive));
                    });
                }
                
                return;
            }

            var directoryInfo = new DirectoryInfo(CurrentDirectoryPathName);

            var bw = sender as BackgroundWorker;

            try
            {
                var directories = directoryInfo.EnumerateDirectories()
                    .OrderBy(d => d.Name, new NaturalSortComparer());

                foreach (var directory in directories)
                {
                    if (bw.CancellationPending)
                    {
                        e.Cancel = true;

                        return;
                    }

                    await _synchronizationHelper.InvokeAsync(() =>
                    {
                        DirectoriesAndFiles.Add(new DirectoryViewModel(directory));
                    });
                }

                foreach (var fileInfo in directoryInfo.EnumerateFiles().OrderBy(f => f.Name, new NaturalSortComparer()))
                {
                    if (bw.CancellationPending)
                    {
                        e.Cancel = true;

                        return;
                    }

                    await _synchronizationHelper.InvokeAsync(() =>
                    {
                        DirectoriesAndFiles.Add(new FileViewModel(fileInfo));
                    });
                }
            }
            catch (Exception ex)
            {
                //TODO: Try Exception 
            }
        }

        #endregion
    }
}
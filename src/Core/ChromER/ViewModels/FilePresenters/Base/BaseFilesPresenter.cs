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
        public DelegateCommand OpenNewTabCommand { get; }
        public DelegateCommand OpenNewWindowCommand => ChromEr.Instance.OpenNewWindowCommand;

        #endregion

        #region Constructor

        protected BaseFilesPresenter(DirectoryTabItemViewModel directoryTabItemView, string directoryPathName)
        {
            _synchronizationHelper = directoryTabItemView.SynchronizationHelper;

            CurrentDirectoryPathName = directoryPathName;

            _backgroundWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };

            OpenCommand = new DelegateCommand(Open);
            OpenNewTabCommand = new DelegateCommand(OpenNewTab);
       
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

        private void OpenNewTab(object parameter)
        {
            if (parameter is object[] {Length: 2} parameters &&
                parameters[0] is MainViewModel mainViewModel &&
                parameters[1] is FileEntityViewModel fileEntityViewModel)
            {
                mainViewModel.OnOpenNewTab(fileEntityViewModel);
            }
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
                var group = "Папки";

                var specialFolders = new[]
                {
                    Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
                };

                foreach (var specialFolder in specialFolders)
                {
                    await _synchronizationHelper.InvokeAsync(() =>
                    {
                        DirectoriesAndFiles.Add(new DirectoryViewModel(new DirectoryInfo(specialFolder))
                        {
                            Group = group
                        });
                    });
                }

                group = "Устройства и диски";

                foreach (var logicalDrive in Directory.GetLogicalDrives())
                {
                    await _synchronizationHelper.InvokeAsync(() =>
                    {
                        DirectoriesAndFiles.Add(new LogicalDriveViewModel(logicalDrive)
                        {
                            Group = group
                        });
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
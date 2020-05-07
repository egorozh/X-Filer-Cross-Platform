using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace Explorer.Shared.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Public Properties

        public string FilePath { get; set; }

        public ObservableCollection<FileEntityViewModel> DirectoriesAndFiles { get; set; } =
            new ObservableCollection<FileEntityViewModel>();

        public FileEntityViewModel SelectedFileEntity { get; set; }

        #endregion

        #region Commands

        public ICommand OpenCommand { get; }

        #endregion

        #region Events

        #endregion

        #region Constructor

        public MainViewModel()
        {
            OpenCommand = new DelegateCommand(Open);

            foreach (var logicalDrive in Directory.GetLogicalDrives())
                DirectoriesAndFiles.Add(new DirectoryViewModel(logicalDrive));
               
        }

        #endregion

        #region Protected Methods

        #endregion

        #region Commands Methods

        private void Open(object parameter)
        {
            if (parameter is DirectoryViewModel directoryViewModel)
            {
                FilePath = directoryViewModel.FullName;

                DirectoriesAndFiles.Clear();

                var directoryInfo = new DirectoryInfo(FilePath);

                foreach (var directory in directoryInfo.GetDirectories())
                {
                    DirectoriesAndFiles.Add(new DirectoryViewModel(directory));
                }

                foreach (var fileInfo in directoryInfo.GetFiles())
                {
                    DirectoriesAndFiles.Add(new FileViewModel(fileInfo));
                }
            }

        }

        #endregion
    }
}
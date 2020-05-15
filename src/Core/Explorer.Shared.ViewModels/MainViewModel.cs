using System.Collections.ObjectModel;
using System.Linq;

namespace Explorer.Shared.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Public Properties

        public ObservableCollection<DirectoryTabItemViewModel> DirectoryTabItems { get; set; } =
            new ObservableCollection<DirectoryTabItemViewModel>();

        public DirectoryTabItemViewModel CurrentDirectoryTabItem { get; set; }

        #endregion

        #region Commands

        public DelegateCommand AddTabItemCommand { get; }

        public DelegateCommand CloseCommand { get; }

        #endregion

        #region Events

        #endregion

        #region Constructor

        public MainViewModel()
        {
            AddTabItemCommand = new DelegateCommand(OnAddTabItem);
            CloseCommand = new DelegateCommand(OnClose);

            AddTabItemViewModel();
        }

        #endregion

        #region Public Methods

        public void ApplicationClosing()
        {
            
        }

        #endregion

        #region Commands Methods

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
            var vm = new DirectoryTabItemViewModel();
            
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
}
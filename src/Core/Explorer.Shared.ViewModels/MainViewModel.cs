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

        #endregion

        #region Events

        #endregion

        #region Constructor

        public MainViewModel()
        {
            AddTabItemCommand = new DelegateCommand(OnAddTabItem);

            AddTabItemViewModel();
        }

        #endregion

        #region Protected Methods

        #endregion

        #region Commands Methods

        private void OnAddTabItem(object obj)
        {
            AddTabItemViewModel();
        }

        #endregion

        #region Private Methods

        private void AddTabItemViewModel()
        {
            var vm = new DirectoryTabItemViewModel();

            vm.Closed += Vm_Closed;

            DirectoryTabItems.Add(vm);
            CurrentDirectoryTabItem = vm;
        }

        private void Vm_Closed(object sender, System.EventArgs e)
        {
            if (sender is DirectoryTabItemViewModel directoryTabItemViewModel)
            {
                CloseTab(directoryTabItemViewModel);
            }
        }

        private void CloseTab(DirectoryTabItemViewModel directoryTabItemViewModel)
        {
            directoryTabItemViewModel.Closed -= Vm_Closed;

            DirectoryTabItems.Remove(directoryTabItemViewModel);

            CurrentDirectoryTabItem = DirectoryTabItems.FirstOrDefault();
        }

        #endregion
    }
}
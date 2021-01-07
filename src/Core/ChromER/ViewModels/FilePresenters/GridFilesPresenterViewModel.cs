namespace ChromER
{
    public class GridFilesPresenterViewModel : BaseFilesPresenter
    {
        public GridFilesPresenterViewModel(ISynchronizationHelper synchronizationHelper, string directoryPathName) :
            base(synchronizationHelper, directoryPathName)
        {
        }
    }
}
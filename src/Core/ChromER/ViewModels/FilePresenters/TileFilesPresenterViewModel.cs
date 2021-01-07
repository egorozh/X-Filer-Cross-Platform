namespace ChromER
{
    public class TileFilesPresenterViewModel : BaseFilesPresenter
    {
        public TileFilesPresenterViewModel(ISynchronizationHelper synchronizationHelper, string directoryPathName) :
            base(
                synchronizationHelper, directoryPathName)
        {
        }
    }
}
namespace ChromER.SDK
{
    public class TileFilesPresenterViewModel : BaseFilesPresenter
    {
        public TileFilesPresenterViewModel(string directoryPathName,
            IFileEntityFactory fileEntityFactory,
            IWindowFactory windowFactory) :
            base(fileEntityFactory, windowFactory, directoryPathName)
        {
        }
    }
}
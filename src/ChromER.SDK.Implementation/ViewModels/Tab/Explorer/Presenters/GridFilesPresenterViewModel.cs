namespace ChromER.SDK
{
    public class GridFilesPresenterViewModel : BaseFilesPresenter
    {
        public GridFilesPresenterViewModel(string directoryPathName,
            IFileEntityFactory fileEntityFactory,
            IWindowFactory windowFactory) :
            base(fileEntityFactory,windowFactory, directoryPathName)
        {
        }
    }
}
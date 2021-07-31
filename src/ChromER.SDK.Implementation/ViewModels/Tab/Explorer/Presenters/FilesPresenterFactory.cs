namespace ChromER.SDK
{
    public class FilesPresenterFactory : IFilesPresenterFactory
    {
        private readonly IFileEntityFactory _fileEntityFactory;
        private readonly IWindowFactory _windowFactory;

        public FilesPresenterFactory(IFileEntityFactory fileEntityFactory,
            IWindowFactory windowFactory)
        {
            _fileEntityFactory = fileEntityFactory;
            _windowFactory = windowFactory;
        }

        public IFilesPresenter CreatePresenter(PresenterType presenterType, string currentDirectory)
            => presenterType switch
            {
                PresenterType.Grid => new GridFilesPresenterViewModel(currentDirectory, _fileEntityFactory,
                     _windowFactory),

                _ => new TileFilesPresenterViewModel(currentDirectory, _fileEntityFactory, _windowFactory)
            };
    }
}
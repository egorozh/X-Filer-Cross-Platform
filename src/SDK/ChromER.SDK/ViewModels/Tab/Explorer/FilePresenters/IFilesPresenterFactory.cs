namespace ChromER.SDK
{
    public interface IFilesPresenterFactory
    {
        IFilesPresenter CreatePresenter(PresenterType presenterType, string currentDirectory);
    }
}
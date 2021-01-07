using System;

namespace ChromER
{
    public interface IFilesPresenter : IDisposable
    {
        string CurrentDirectoryPathName { get; }

        event EventHandler<OpenDirectoryEventArgs> DirectoryOrFileOpened;
    }
}
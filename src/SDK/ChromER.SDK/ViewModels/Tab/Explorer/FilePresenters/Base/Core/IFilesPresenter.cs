using System;

namespace ChromER.SDK
{
    public interface IFilesPresenter : IDisposable
    {
        string CurrentDirectoryPathName { get; }

        event EventHandler<OpenDirectoryEventArgs> DirectoryOrFileOpened;
    }
}
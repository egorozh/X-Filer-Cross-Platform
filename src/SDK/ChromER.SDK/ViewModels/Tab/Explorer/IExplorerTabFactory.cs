using System.IO;
using Dock.Model.Controls;

namespace ChromER.SDK
{
    public interface IExplorerTabFactory
    {
        IDocument CreateExplorerTab(DirectoryInfo directoryInfo);
        IDocument CreateExplorerTab(string dirPath, string name);
        IDocument CreateRootTab();
    }
}
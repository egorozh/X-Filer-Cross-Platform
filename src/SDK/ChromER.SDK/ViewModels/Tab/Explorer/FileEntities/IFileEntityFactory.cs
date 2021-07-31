using System.IO;

namespace ChromER.SDK
{
    public interface IFileEntityFactory
    {
        LogicalDriveViewModel CreateLogicalDrive(string logicalDrive, string? @group = null);
        DirectoryViewModel CreateDirectory(DirectoryInfo directoryInfo, string? @group = null);
        FileViewModel CreateFile(FileInfo fileInfo, string? @group = null);
    }
}
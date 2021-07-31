using System.IO;

namespace ChromER.SDK
{
    public sealed class LogicalDriveViewModel : DirectoryViewModel
    {
        public LogicalDriveViewModel(string directoryName, IIconLoader iconLoader)
            : base(new DirectoryInfo(directoryName), iconLoader)
        {
            FullName = directoryName;
        }

        public override string GetRootName()
            => FullName;
    }
}
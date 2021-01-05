using System.IO;

namespace ChromER
{
    public sealed class LogicalDriveViewModel : DirectoryViewModel
    {
        public LogicalDriveViewModel(string directoryName) : base(new DirectoryInfo(directoryName))
        {
            FullName = directoryName;
        }
        
        public override string GetRootName()
            => FullName;
    }
}
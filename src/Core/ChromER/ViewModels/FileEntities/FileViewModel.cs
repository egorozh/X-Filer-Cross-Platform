using System.IO;

namespace ChromER
{
    public sealed class FileViewModel : FileEntityViewModel
    {
        public FileViewModel(string name) : base(name)
        {
        }

        public FileViewModel(FileInfo fileInfo) : base(fileInfo.Name)
        {
            FullName = fileInfo.FullName;
        }

        public override string GetRootName()
            => new FileInfo(FullName).Directory?.Root.Name;
    }
}
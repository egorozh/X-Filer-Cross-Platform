using System.IO;

namespace ChromER
{
    public sealed class FileViewModel : FileEntityViewModel
    {
        private readonly FileInfo _fileInfo;

        public double Size => _fileInfo.Length / 1024.0;

        public FileViewModel(FileInfo fileInfo) : base(fileInfo.Name)
        {
            _fileInfo = fileInfo;
            FullName = fileInfo.FullName;
        }

        public override string ChangeDateTime => _fileInfo.LastWriteTime.ToString();

        public override string GetRootName()
            => new FileInfo(FullName).Directory?.Root.Name;
    }
}
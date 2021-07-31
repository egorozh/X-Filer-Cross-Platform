using System.IO;

namespace ChromER.SDK
{
    public interface IIconPathProvider
    {
        FileInfo GetIconPath(FileEntityViewModel viewModel);
    }
}
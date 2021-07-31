using Avalonia.Media.Imaging;

namespace ChromER.SDK
{
    public interface IIconLoader
    {
        Bitmap? GetIcon(FileEntityViewModel viewModel);
    }
}
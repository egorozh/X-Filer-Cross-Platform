using System.Collections.Generic;

namespace ChromER
{
    /// <summary>
    /// Менеджер закладок
    /// </summary>
    public interface IBookmarksManager
    {
        IReadOnlyCollection<MenuItemViewModel> Bookmarks { get; }
        DelegateCommand AddBookmarkCommand { get; }
    }
}
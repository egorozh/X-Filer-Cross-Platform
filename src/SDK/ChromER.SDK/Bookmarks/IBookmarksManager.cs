using Prism.Commands;
using System.Collections.Generic;

namespace ChromER.SDK
{
    /// <summary>
    /// Менеджер закладок
    /// </summary>
    public interface IBookmarksManager
    {
        IReadOnlyCollection<IMenuItemViewModel> Bookmarks { get; }
        DelegateCommand<string> AddBookmarkCommand { get; }
    }
}
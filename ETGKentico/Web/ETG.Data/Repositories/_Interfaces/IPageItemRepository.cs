using ETG.Data.Models.PageTypes;

namespace ETG.Data.Repositories
{
    public interface IPageItemRepository
    {
        PageItemModel GetPage(string path);
    }
}

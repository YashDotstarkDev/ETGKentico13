using ETG.Data.Models.PageTypes;

namespace ETG.Data.Repositories
{
    public interface IContainerRepository 
    {
        PageItemModel GetContainer(string path);
    }
}

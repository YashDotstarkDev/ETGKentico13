using ETG.Data.Models.Pages;

namespace ETG.Data.Repositories.Pages
{
    public interface IOrderCancelPageRepository
    {
        OrderCancelPageModel GetContents();
    }
}

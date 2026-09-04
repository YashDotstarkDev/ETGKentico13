using ETG.Data.Career.Models;

namespace ETG.Data.Career.Repositories
{
    public interface ICareerApplyPageRepository 
    {
        CareerApplyPageModel GetApplyPage(string roleAlias);
    }
}

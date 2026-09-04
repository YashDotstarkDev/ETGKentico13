using ETG.Data.Career.Models;

namespace ETG.Data.Career.Services
{
    public interface ICareerService
    {
        CareerRoleModel GetCareerRole(string path);
    }
}

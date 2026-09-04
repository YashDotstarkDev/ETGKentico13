using ETG.Data.Career.Models;
using System.Collections.Generic;

namespace ETG.Data.Career.Repositories
{
    public interface ICareerRoleRepository
    {
        CareerRoleModel GetCareerRole(string path);
        List<CareerRoleBasicInfoModel> GetCareerRoles(string path);
    }
}

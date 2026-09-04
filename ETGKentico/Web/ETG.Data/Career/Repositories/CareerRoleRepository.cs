using ETG.Core.Kentico;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Career.Models;
using System.Collections.Generic;
using System.Linq;

namespace ETG.Data.Career.Repositories
{
    public class CareerRoleRepository : ICareerRoleRepository
    {
        private readonly ISiteContext _siteContext;
        public CareerRoleRepository(ISiteContext siteContext)
        {
            _siteContext = siteContext;
        }
        public CareerRoleModel GetCareerRole(string path)
        {
            return CareerRoleProvider.GetCareerRole(path, _siteContext.CurrentCultureCode, _siteContext.SiteName)
                   .Select(a => new CareerRoleModel
                   {
                       BasicInfo = new CareerRoleBasicInfoModel
                       {
                           Title = a.CareerRoleTitle,
                           Location = a.CareerRoleLocation,
                           PositionType = a.CareerRolePositionType,
                           Path = a.NodeAliasPath
                       },
                       Description = a.CareerRoleDescription,
                       SummaryOfPosition = a.CareerRoleSummaryOfPosition,
                       DutiesAndResponsibilities = a.CareerRoleDutiesAndResponsibilities,
                       Address = a.CareerRoleAddress,
                       State = a.CareerRoleState,
                       PageTitle = a.DocumentPageTitle,
                       PageDescription = a.DocumentPageDescription,
                       PageAliasPath = a.NodeAliasPath,
                       PageKeywords = a.DocumentPageKeyWords,
                       PageAlias = a.NodeAlias,
                       DocumentID = a.DocumentID

                   }
                   ).FirstOrDefault();
        }

        public List<CareerRoleBasicInfoModel> GetCareerRoles(string path)
        {
            return CareerRoleProvider.GetCareerRoles().Path(path, CMS.DocumentEngine.PathTypeEnum.Children).OnCurrentSite()
                .Select(a => new CareerRoleBasicInfoModel
                {
                    Title = a.CareerRoleTitle,
                    Location = a.CareerRoleLocation,
                    PositionType = a.CareerRolePositionType,
                    Path = $"/careers/{a.NodeAlias}",
                    DateModified = a.DocumentModifiedWhen,
                    DocumentSearchExcluded = a.DocumentSearchExcluded
                }).ToList();
        }
    }
}

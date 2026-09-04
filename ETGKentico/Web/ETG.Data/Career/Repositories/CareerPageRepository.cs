using ETG.Core.Constants;
using ETG.Data.Cache;
using ETG.Data.Career.Models;
using ETG.Data.Repositories;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;

namespace ETG.Data.Career.Repositories
{
    public class CareerPageRepository : BasePageRepository, ICareerPageRepository
    {
        private readonly IPageItemRepository _pageRepository;
        private readonly ICareerRoleRepository _careerRoleRepository;
        private readonly ICacheService _cacheService;
        public CareerPageRepository(IPageItemRepository pageRepository, ICareerRoleRepository careerRoleRepository, ICacheService cacheService, IImageRepository imageRepository, IShareLinksService shareLinksService)
            : base(imageRepository, cacheService, shareLinksService)
        {
            _careerRoleRepository = careerRoleRepository;
            _pageRepository = pageRepository;
            _cacheService = cacheService;
        }
        public CareerPageModel Get(string url, string path = "")
        {
            var page = _cacheService.GetDocumentDependentOnPath(() => _pageRepository.GetPage(PathConstants.PATH_CAREERS), "CareersPage", PathConstants.PATH_CAREERS);

            if (page == null)
            {
                return null;
            }

            return new CareerPageModel
            {
                Page = page,
                Roles = _cacheService.GetDocumentDependentOnChildrenPath(() => _careerRoleRepository.GetCareerRoles(PathConstants.PATH_CAREER_ROLES), "CareerRoles", PathConstants.PATH_CAREER_ROLES),
                BreadCrumbs = GetBreadCrumbs("Careers")
            };
        }
    }
}

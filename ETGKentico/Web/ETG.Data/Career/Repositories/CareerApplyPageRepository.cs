using ETG.Core.Constants;
using ETG.Data.Cache;
using ETG.Data.Career.Models;
using ETG.Data.Career.Services;
using ETG.Data.Repositories;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;

namespace ETG.Data.Career.Repositories
{
    public class CareerApplyPageRepository : BasePageRepository, ICareerApplyPageRepository
    {
        private readonly IPageItemRepository _pageRepository;
        private readonly ICareerService _careerService;
        private readonly ICacheService _cacheService;
        public CareerApplyPageRepository(IPageItemRepository pageRepository, ICareerService careerService, ICacheService cacheService, IImageRepository imageRepository, IShareLinksService shareLinksService)
            : base(imageRepository, cacheService, shareLinksService)
        {
            _careerService = careerService;
            _pageRepository = pageRepository;
            _cacheService = cacheService;
        }
        public CareerApplyPageModel GetApplyPage(string roleAlias)
        {
            var page = _cacheService.GetDocumentDependentOnPath(() => _pageRepository.GetPage(PathConstants.PATH_CAREERS_APPLY), "CareersApplyPage", PathConstants.PATH_CAREERS_APPLY);

            if (page == null)
            {
                return null;
            }

            var role = _careerService.GetCareerRole($"{PathConstants.PATH_CAREER_ROLES}/{roleAlias}");
            var viewModel = new CareerApplyPageModel
            {
                Page = page
            };

            if (role != null && role.BasicInfo != null)
            {
                viewModel.Form = new CareerApplyFormModel
                {
                    Role = role.BasicInfo.Title,
                    PageAlias = role.PageAlias
                };
                viewModel.Role = role;
                viewModel.BreadCrumbs = GetBreadCrumbs(role.BasicInfo.Title, $"/careers/{role.PageAlias}", "Apply");
            }

            return viewModel;
        }
    }
}

using Castle.Core.Internal;
using Devotion.Web.Base.Extensions;
using ETG.Core.Constants;
using ETG.Data.Brochure.Models;
using ETG.Data.Cache;
using ETG.Data.Repositories;
using System.Linq;
using ETG.Data.Brochure.Services;
using ETG.Data.Models.PageTypes;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;

namespace ETG.Data.Brochure.Repositories
{
    public class BrochureListingPageRepository : BasePageRepository, IBrochureListingPageRepository
    {
        private readonly IPageItemRepository _pageRepository;
        private readonly IBrochureService _brochureService;
        private readonly ICacheService _cacheService;
        public BrochureListingPageRepository(IPageItemRepository pageRepository, IBrochureService brochureService, ICacheService cacheService, IImageRepository imageRepository,
            IShareLinksService shareLinksService) : base(imageRepository, cacheService, shareLinksService)
        {
            _brochureService = brochureService;
            _pageRepository = pageRepository;
            _cacheService = cacheService;
        }
        public BrochureListingPageModel Get(string url, string path = "")
        {
            var page = _cacheService.GetDocumentDependentOnPath(() => _pageRepository.GetPage(url), "brochurelanding", url);

            if (page == null)
            {
                return null;
            }

            return new BrochureListingPageModel
            {
                Page = page,
                Brochures = _brochureService.GetAllBrochures(),
                BreadCrumbs = GetBreadCrumbs("Brochures")
            };


        }

        public BrochurePageModel GetBrochurePage(string url, string alias = "")
        {

            var brochure = _brochureService.GetBrochure(alias);

            if (brochure == null)
            {
                return null;
            }
            return new BrochurePageModel
            {
                Brochure = brochure,
                BreadCrumbs = GetBreadCrumbs("Brochures", "/brochures", brochure.Name)
            };


        }

        public BrochureOrderPageModel GetBrochureOrderPage(string brochureGuids)
        {
            var page = _cacheService.GetDocumentDependentOnPath(() => _pageRepository.GetPage(PathConstants.PATH_BROCHURE_ORDER), "brochureorder", PathConstants.PATH_BROCHURE_ORDER);

            if (page == null)
            {
                return null;
            }
            

            var model = new BrochureOrderPageModel
            {
                Page = page,
            };

            if (!brochureGuids.IsNullOrEmpty())
            {
                var guidList = brochureGuids.Split(',').Where(a => a.IsGuid()).Select(a => a.ToGuid()).ToList();
                var orders = _brochureService.GetBrochures(guidList).ToList();
                if (!orders.IsNullOrEmpty())
                {
                    model.Form = new BrochureOrderFormModel
                    {
                        BrochureOrders = orders,
                        BrochureGuids = string.Join(",", orders.Select(a => a.BrochureNodeGuid)).Replace(" ", "").Trim()
                    };
                }
            }

            return model;
        }
    }
}

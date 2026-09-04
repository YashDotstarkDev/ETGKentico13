using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;
using CMS.CustomTables;
using ETG.Core.Constants;
using ETG.Core.CustomTables;
using ETG.Core.Kentico;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Brochure.Models;
using ETG.Data.Cache;
using ETG.Data.Models.Base;
using ETG.Data.Models.Pages;
using ETG.Data.Models.PageTypes;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;
using ETG.Data.Models.Forms;

namespace ETG.Data.Repositories
{
    public class NewsletterRepository : BasePageRepository, INewsletterRepository
    {
        private readonly IPageItemRepository _pageRepository;
        private readonly ISiteContext _siteContext;
        private readonly ICacheService _cacheService;
        public NewsletterRepository(IPageItemRepository pageRepository, ISiteContext siteContext, ICacheService cacheService, IShareLinksService shareLinksService, IImageRepository imageRepository) : base(imageRepository, cacheService, shareLinksService)
        {
            _siteContext = siteContext;
            _cacheService = cacheService;
            _pageRepository = pageRepository;
        }
        public UnsubscribePageModel GetUnsubscribePage(string path)
        {
            var page = _cacheService.GetDocumentDependentOnPath(() => _pageRepository.GetPage(path), "newsletterunsubscribe", path);

            if (page == null)
            {
                return null;
            }

            return new UnsubscribePageModel
            {
                Page = page
            };

        }
    }
}

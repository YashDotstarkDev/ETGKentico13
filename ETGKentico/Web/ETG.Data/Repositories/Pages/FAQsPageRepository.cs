using ETG.Data.Cache;
using ETG.Data.DestinationExpertTeam.Services;
using ETG.Data.Models.Common;
using ETG.Data.Models.Pages;
using ETG.Data.Repositories._Interfaces;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;

namespace ETG.Data.Repositories.Pages
{
    public class FAQsPageRepository : BasePageRepository, IFAQsPageRepository
    {
        private ICacheService _cacheService;
        private IPageItemRepository _pageRepository;
        private IContactService _contactService;
        private readonly IDestinationExpertTeamService _expertTeamService;

        public FAQsPageRepository(ICacheService cacheService, IPageItemRepository pageRepository,IContactService contactService, 
            IDestinationExpertTeamService expertTeamService, 
            IImageRepository imageRepository,
            IShareLinksService shareLinksService): base(imageRepository, cacheService, shareLinksService)
        {
            _cacheService = cacheService;
            _pageRepository=pageRepository;
            _contactService = contactService;
            _expertTeamService = expertTeamService;
        }

        public FAQsPageModel Get(string url, string path = "")
        {
            var page = _cacheService.GetDocumentDependentOnPath(() => _pageRepository.GetPage(url), "faqspage", url);

            if (page == null)
            {
                return null;
            }
            var model = new FAQsPageModel
            {
                Page = page,
                ContactDetails = new GenericSideContactModel
                {
                    GeneralContact = _contactService.GetETGContactInfo(),
                    DestinationExpertContacts = _expertTeamService.GetAllDestinationExperts()
                },
                BreadCrumbs = GetBreadCrumbs("FAQs")
            };
            return model;
                 
        }
    }
}
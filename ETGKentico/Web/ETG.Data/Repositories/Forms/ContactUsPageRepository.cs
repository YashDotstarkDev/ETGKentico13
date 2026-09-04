using AutoMapper;
using CMS.EventLog;
using ETG.Data.Cache;
using ETG.Data.Configuration;
using ETG.Data.DestinationExpertTeam.Services;
using ETG.Data.Models.Common;
using ETG.Data.Models.Forms;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;

namespace ETG.Data.Repositories.Forms
{
    public class ContactUsPageRepository : BasePageRepository, IContactUsPageRepository
    {
        private readonly IMapper _mapper;
        private readonly IPageItemRepository _pageRepository;
        private readonly ICacheService _cacheService;
        private readonly IContactService _contactService;
        private readonly IDestinationExpertTeamService _expertTeamService;
        public ContactUsPageRepository(IPageItemRepository pageRepository, IContactService contactService,
        IDestinationExpertTeamService expertTeamService, ICacheService cacheService, 
        IMapper mapper, IShareLinksService shareLinksService, IImageRepository imageRepository) : base(imageRepository, cacheService, shareLinksService)
        {
            _mapper = mapper;
            _cacheService = cacheService;
            _pageRepository = pageRepository;
            _contactService = contactService;
            _expertTeamService = expertTeamService;
        }
        public ContactUsPageModel Get(string url, string path = "")
        {
            var page = _cacheService.GetDocumentDependentOnPath(() => _pageRepository.GetPage(url), "genericenquire", url);

            if (page == null)
            {
                return null;
            }
            var model = new ContactUsPageModel
            {
                Page = page,
                ContactDetails = new GenericSideContactModel
                {
                    GeneralContact = _contactService.GetETGContactInfo(),
                    DestinationExpertContacts = _expertTeamService.GetAllDestinationExperts()
                },
                BreadCrumbs = GetBreadCrumbs("Contact us")
            };
            return model;
                 
        }
    }
}

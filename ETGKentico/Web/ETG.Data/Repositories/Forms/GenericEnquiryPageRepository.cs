using AutoMapper;
using ETG.Data.Experience.Repositories;
using ETG.Data.Models.Forms;
using System.Linq;
using System.Web.Mvc;
using ETG.Core.Constants;
using ETG.Data.Cache;
using ETG.Data.Destination.Services;
using ETG.Data.DestinationExpertTeam.Models;
using ETG.Data.Models.Common;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;

namespace ETG.Data.Repositories.Forms
{
    public class GenericEnquiryPageRepository : BasePageRepository, IGenericEnquiryPageRepository
    {
        private readonly IMapper _mapper;
        private readonly IContainerRepository _containerRepository;
        private readonly ICacheService _cacheService;
        private readonly IDestinationService _destinationService;
        private readonly IExperienceRepository _experienceRepository;
        private IContactService _contactService;

        public GenericEnquiryPageRepository(
            IContainerRepository containerRepository,
            IContactService contactService,
            IDestinationService destinationService,
            IExperienceRepository experienceRepository,
            ICacheService cacheService,
            IMapper mapper,
            IShareLinksService shareLinksService,
            IImageRepository imageRepository)
            : base(imageRepository, cacheService, shareLinksService)
        {
            _mapper = mapper;
            _cacheService = cacheService;
            _containerRepository = containerRepository;
            _destinationService = destinationService;
            _experienceRepository = experienceRepository;
            _contactService = contactService;
        }

        public GenericEnquiryPageModel Get(string url, string path = "")
        {
            var page = _cacheService.GetDocumentDependentOnPath(() => _containerRepository.GetContainer(url), "genericenquire", url);
            if (page == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(page.RedirectTo))
            {
                return new GenericEnquiryPageModel()
                {
                    Page = page
                };
            }
            
            var destinationOptions =  _destinationService.GetMainDestinations(true)
                .Select(a => new SelectListItem { Text = a.Name, Value = a.Name }).ToList();
            destinationOptions.Insert(0, new SelectListItem
            {
                Text = "Select destination",
                Value = string.Empty
            });
            var experienceOptions = _cacheService.GetDocumentDependentOnChildrenPath
                  (() => _experienceRepository.GetExperiences(PathConstants.PATH_EXPERIENCES), "experienceall", url)
                .OrderBy(a => a.Name).Select(a => new SelectListItem { Text = a.Name, Value = a.Name }).ToList();

            experienceOptions.Insert(0, new SelectListItem
            {
                Text = "Select experience",
                Value = string.Empty
            });
            
            var contact = _contactService.GetETGContactInfo();
            var model = new GenericEnquiryPageModel
            {
                Page = page,
                Destinations = destinationOptions,
                Experiences = experienceOptions,
                ContactDetails = new GenericSideContactModel
                {
                    GeneralContact = contact
                },
                BreadCrumbs = GetBreadCrumbs("Enquire")
            };

            if (contact != null)
            {
                model.DestinationExpertDetails = new DestinationExpertTeamSummaryModel
                {
                    HeroImage = contact.PreferPhoneBackgroundImage,
                    Description = contact.PreferPhoneDescription,
                    RHSText = contact.PreferPhoneOpeningHoursText,
                    Url = PathConstants.DESTINATION_EXPERTS,
                    Phone = contact.PhoneNumber
                };
            }

            return model;
        }
    }
}
using System;
using System.Collections.Generic;
using ETG.Data.Models.PageTypes;
using System.Linq;
using System.Web.Mvc;
using CMS.DocumentEngine;
using CMS.Helpers;
using ETG.Core.Constants;
using ETG.Core.PageTypes;
using ETG.Data.Cache;
using ETG.Data.Destination.Services;
using ETG.Data.Experience.Repositories;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Models.Forms;
using ETG.Data.Repositories.Common;

namespace ETG.Data.Repositories
{
    public class ContainerRepository : IContainerRepository
    {
        private readonly ICacheService _cacheService;
        private readonly ICTAIconRepository _ctaIconRepository;
        private readonly IDestinationService _destinationService;
        private readonly IExperienceRepository _experienceRepository;

        public ContainerRepository(
            ICacheService cacheService,
            ICTAIconRepository ctaIconRepository,
            IDestinationService destinationService,
            IExperienceRepository experienceRepository)
        {
            _cacheService = cacheService;
            _ctaIconRepository = ctaIconRepository;
            _destinationService = destinationService;
            _experienceRepository = experienceRepository;
        }

        public PageItemModel GetContainer(string path)
        {
            var containerDoc = DocumentHelper
                .GetDocuments<Container>()
                .OnCurrentSite()
                .PublishedVersion()
                .Published()
                .Path(path)
                .FirstOrDefault();

            if (containerDoc == null)
            {
                return null;
            }
            
            if (!string.IsNullOrWhiteSpace(containerDoc.ContainerRedirect) && 
                containerDoc.ContainerExpiry > DateTimeHelper.ZERO_TIME &&
                DateTime.Now > containerDoc.ContainerExpiry)
            {
                return new PageItemModel
                {
                    RedirectTo = containerDoc.ContainerRedirect
                };
            }

            var container = new PageItemModel
            {
                PageName = containerDoc.DocumentName,
                IsNewLandingPage = containerDoc.ContainerIsNewCampaignLandingPage,
                FormEnabled = containerDoc.ContainerFormEnabled,
                FormTitle = containerDoc.ContainerFormTitle,
                PageHero = new PageHeroModel
                {
                    HeadingSummary = containerDoc.ContainerHeadingText,
                    Heading = containerDoc.ContainerHeading,
                    HeroCaption = containerDoc.ContainerHeroAccreditation,
                    HeroImage = containerDoc.ContainerHeroImage,
                    HeroImageAltText = containerDoc.ContainerHeroImageAltTag,
                    HideShareButton = containerDoc.ContainerHideShareButton,
                    HeroContactUsCtaPath = containerDoc.ContainerHeroContactUsCtaPath,
                    HeroPhone = containerDoc.ContainerHeroPhone
                },
                ProofPointsComponent = new ProofPointComponentModel
                {
                    Title = containerDoc.ContainerProofPointsTitle
                },
                Page = new PageNodeModel
                {
                    DocumentID = containerDoc.DocumentID,
                    PageTitle = containerDoc.DocumentPageTitle,
                    PageDescription = containerDoc.DocumentPageDescription,
                    PageAliasPath = containerDoc.NodeAliasPath,
                    PageAlias = containerDoc.NodeAlias,
                    PageKeywords = containerDoc.DocumentPageKeyWords,
                    ShareTitle = containerDoc.DocumentPageTitle,
                    ShareDescription = containerDoc.DocumentPageDescription,
                    ShareImage = containerDoc.ContainerHeroImage,
                    ExcludedFromSearch = containerDoc.DocumentSearchExcluded,
                    
                },
                JsonSchema = containerDoc.ContainerJsonSchema
            };

            if (container.FormEnabled)
            {
                var enquiryPage = GetEnquiryPage("/Enquire");

                container.Destinations = enquiryPage.Destinations;
                container.Experiences = enquiryPage.Experiences;
            }

            if (!string.IsNullOrWhiteSpace(container.ProofPointsComponent?.Title))
            {
                var cmsProofPoints = _cacheService
                    .GetDocumentDependentOnChildrenPath(
                        () => _ctaIconRepository.Get(containerDoc.ContainerProofPointsFolder, ""),
                        "proofpoints",
                        containerDoc.ContainerProofPointsFolder);

                var proofPoints = new List<CTAIconModel>();
                foreach (var item in cmsProofPoints)
                {
                    if (proofPoints.Any(x => x.Label.Equals(item.Label)))
                    {
                        continue;
                    }
                    
                    proofPoints.Add(item);
                }

                container.ProofPointsComponent.ProofPoints = proofPoints;
            }

            return container;
        }

        private GenericEnquiryPageModel GetEnquiryPage(string url, string path = "")
        {
            var page = _cacheService.GetDocumentDependentOnPath(() => GetContainer(url), "genericenquire", url);
            if (page == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(page.RedirectTo))
            {
                return new GenericEnquiryPageModel
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
            
            var model = new GenericEnquiryPageModel
            {
                Page = page,
                Destinations = destinationOptions,
                Experiences = experienceOptions,
                ContactDetails = new GenericSideContactModel()
            };

            return model;
        }
    }
}
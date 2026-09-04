using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using CMS.Base;
using ETG.Core.Kentico;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Models.Competition;
using ETG.Data.Models.PageTypes;

namespace ETG.Data.Repositories
{
    public class HomepageRepository : IHomepageRepository
    {
        private readonly ISiteContext _siteContext;
        public HomepageRepository(ISiteContext siteContext)
        {
            _siteContext = siteContext;
        }
        public HomepageModel GetHomepage(string path)
        {
            return HomepageProvider.GetHomepages().Path(path).OnCurrentSite().Select(a=>new HomepageModel
            {
                DocumentID = a.DocumentID,
                HeroImage = a.HomepageHeroImage,
                HeroVideo = a.HomepageHeroVideo,
                HeroVideoImage = a.HomepageHeroVideoImage,
                HeroCaption = a.HomepageHeroCaption,
                HeroSubCaption = a.HomepageHeroSubCaption,
                HeroSlideShowFolder = a.HomepageHeroSlideShowFolder,
                FeaturedTilesTitle = a.HomepageFeaturedTilesTitle,
                FeaturedTilesFolder = a.HomepageFeaturedTilesFolder,
                ProofPointsTitle = a.HomepageProofPointsTitle,
                ProofPointsFolder = a.HomepageProofPointsFolder,
                DestinationsSectionTitle = a.HomepageDestinationsSectionTitle,
                DestinationExpertGuid = a.HomepageDestinationExpertGuid,
                FeatureTourTitle = a.HomepageFeatureToursSectionTitle,
                FeatureTourCodes = a.HomepageFeatureTours,
                FeatureTourViewAllUrl = a.HomepageFeatureToursViewAllUrl,
                FeatureTourTitle2 = a.HomepageFeatureToursSectionTitle2,
                FeatureTourCodes2 = a.HomepageFeatureTours2,
                FeatureTourViewAllButtonLabel2 = a.HomepageFeatureToursViewAllLabel2,
                FeatureTourViewAllUrl2 = a.HomepageFeatureToursViewAllUrl2,
                TravelBlogsSectionTitle = a.HomepageTravelBlogsSectionTitle,
                TravelBlogsSectionDescription = a.HomepageTravelBlogsSectionDescription,
                TravelBlogsSectionViewAllUrl = a.HomepageTravelBlogsSectionViewAllUrl,
                PageTitle = a.DocumentPageTitle,
                PageDescription = a.DocumentPageDescription,
                PageAliasPath = a.NodeAliasPath,
                PageAlias = a.NodeAlias,
                PageKeywords = a.DocumentPageKeyWords,
                ShareTitle = a.DocumentPageTitle,
                ShareDescription = a.DocumentPageDescription,
                ShareImage = a.HomepageHeroImage,
                ExcludedFromSearch = a.DocumentSearchExcluded,
                JsonSchema = a.HomepageJsonSchema,
                PopupCompetition = new PopupCompetitionModel
                {
                    PopupTitle = a.HomepagePopupTitle,
                    PopupSubTitle = a.HomepagePopupSubTitle,
                    ShowCompetitionPopup = a.HomepageShowCompetitionPopup,
                    CompetitionCookieName = a.HomepageCompetitionCookieName,
                    CompetitionTermsUrl = a.HomepageCompetitionTermsUrl,
                    PopupThankYouMessage = a.HomepageCompetitionPopupThankYou
                },
                SelectedExperiences = GetExperiencesGuids(a.HomepageExperiences)
            }).FirstOrDefault();
        }

        private List<Guid> GetExperiencesGuids(string experiences)
        {
            return experiences.IsNullOrEmpty() ? new List<Guid>() : experiences.Split(';').Select(a => a.ToGuid(Guid.Empty)).ToList();
        }
    }
}

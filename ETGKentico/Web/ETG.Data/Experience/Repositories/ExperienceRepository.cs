using CMS.DocumentEngine;
using ETG.Core.Kentico;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Experience.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using ETG.Data.Factories;

namespace ETG.Data.Experience.Repositories
{
    public class ExperienceRepository : IExperienceRepository
    {
        private readonly ISiteContext _siteContext;

        public ExperienceRepository(ISiteContext siteContext)
        {
            _siteContext = siteContext;
        }
        public ExperienceModel GetExperience(string path) { 
            return ExperienceProvider.GetExperience(path, _siteContext.CurrentCultureCode, _siteContext.SiteName)
                .OnCurrentSite()
                .Select(a=> new ExperienceModel
                {
                    SummaryInfo = ModelFactory.CreateExperienceSummaryModel(a),
                    Heading = a.ExperienceHeading,
                    HeroIconImage = a.ExperienceIcon,
                    HeroIconSVG = a.ExperienceSVGIcon,
                    HeroImageAccreditation = a.ExperienceHeroImageAccreditation,
                    DocumentID = a.DocumentID,
                    NodeGuid = a.GetGuidValue("NodeGuid", Guid.Empty),
                    FeatureTourCodes = a.ExperienceFeatureTours,
                    FeatureCruiseCodes = a.ExperienceFeatureCruises,
                    PageTitle = a.DocumentPageTitle,
                    PageDescription = a.DocumentPageDescription,
                    PageAliasPath = a.NodeAliasPath,
                    PageAlias = a.NodeAlias,
                    PageKeywords = a.DocumentPageKeyWords,
                    ShareTitle = a.ExperienceName,
                    ShareDescription = a.ExperienceSummary,
                    ShareImage = a.ExperienceHeroImage,
                    ExcludedFromSearch = a.DocumentSearchExcluded,
                    HideGoogleReviews = a.HideGoogleReviews,
                    JsonSchema = a.ExperienceJsonSchema,
                }).FirstOrDefault();
        }

        public List<ExperienceSummaryModel> GetExperiences(string path)
        {
            return ExperienceProvider.GetExperiences().Path(path, PathTypeEnum.Children).OnCurrentSite()
                 .Columns(string.Join(",", nameof(Core.PageTypes.Experience.ExperienceName), nameof(Core.PageTypes.Experience.ExperienceHeroImage), nameof(Core.PageTypes.Experience.NodeAliasPath), nameof(Core.PageTypes.Experience.ExperienceSummary), nameof(Core.PageTypes.Experience.ExperienceIcon)))
                 .OnCurrentSite()
                 .OrderBy("NodeOrder")
                 .Select(a => new ExperienceSummaryModel
                 {
                     Name = a.ExperienceName,
                     Image = a.ExperienceHeroImage,
                     Path = a.NodeAliasPath,
                     Summary = a.ExperienceSummary,
                     IconClass = a.ExperienceIcon
                 }).ToList();
        }
    }
}


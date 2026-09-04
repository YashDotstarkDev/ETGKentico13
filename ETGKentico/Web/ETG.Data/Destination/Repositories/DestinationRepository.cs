using ETG.Core.PageTypes.Providers;
using ETG.Data.Destination.Models;
using System;
using System.Linq;
using ETG.Data.Factories;
using CMS.DocumentEngine;
using System.Globalization;
using ETG.Data.Helpers;

namespace ETG.Data.Destination.Repositories
{
    public class DestinationRepository : IDestinationRepository
    {
        public DestinationModel GetDestination(string path)
        {
            return DestinationProvider.GetDestinations().Path(path).OnCurrentSite().OrderBy("NodeOrder").Select(a => new DestinationModel
            {
                BasicInfo = ModelFactory.CreateDestinationSummaryModel(a),
                JsonSchema = a.DestinationJsonSchema,
                DocumentID = a.DocumentID,
                Detail = a.DestinationDetail,
                WhenToVisit = a.DestinationWhenToVisit,
                ViewPackagesLabel = a.DestinationViewPackagesLabel,
                NodeGuid = a.GetGuidValue("NodeGuid", Guid.Empty),
                FeatureTourCodes = a.DestinationFeatureTours,
                FeatureCruiseCodes = a.DestinationFeatureCruises,
                SVGMap = a.DestinationMapSVG,
                PageTitle = a.DocumentPageTitle,
                PageDescription = a.DocumentPageDescription,
                PageAliasPath = a.NodeAliasPath,
                PageAlias = a.NodeAlias,
                PageKeywords = a.DocumentPageKeyWords,
                ShareTitle = a.DestinationName,
                ShareDescription = a.DestinationSummary,
                ShareImage = a.DestinationHeroImage,
                ExcludedFromSearch = a.DocumentSearchExcluded,
                HideGoogleReviews=a.HideGoogleReviews,
                RelatedArticlesHeader = a.DestinationRelatedArticlesHeader,
                DestinationStickyModel = new DestinationStickyModel {
                    DisplaySticky = a.DisplaySticky,
                    CampaignDetail1 = a.CampaignDetail1,
                    CampaignDetail2 = a.CampaignDetail2,
                    CampaignDetail3 = a.CampaignDetail3,
                    CountdownLabel = a.CountdownLabel,
                    EndOfCampaign = DateTimeHelper.GetGMTTime(a.EndOfCampaign),
                }
            }).FirstOrDefault();
        }

        public bool IsDestinationUnpublished(string path)
        {
            var destinationNode = DocumentHelper.GetDocuments<ETG.Core.PageTypes.Destination>().Path(path).OnCurrentSite().FirstOrDefault();
            return destinationNode != null && !destinationNode.IsPublished;
        }
    }
}

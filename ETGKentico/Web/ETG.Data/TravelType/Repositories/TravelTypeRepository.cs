using System;
using System.Collections.Generic;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Factories;
using ETG.Data.TravelType.Models;
using System.Linq;
using CMS.DocumentEngine;

namespace ETG.Data.TravelType.Repositories
{
    public class TravelTypeRepository : ITravelTypeRepository
    {
        public TravelTypeDetailModel GetTravelTypeDetail(string path)
        {
            return TravelTypeDetailProvider.GetTravelTypeDetails().Path(path).OnCurrentSite().OrderBy("NodeOrder")
                .Select(a => new TravelTypeDetailModel
                {
                    BasicInfo = ModelFactory.CreateTravelTypeDetailSummaryModel(a),
                    NodeGuid = a.GetGuidValue("NodeGuid", Guid.Empty),
                    DocumentID = a.DocumentID,
                    PageTitle = a.DocumentPageTitle,
                    PageDescription = a.DocumentPageDescription,
                    PageAliasPath = a.NodeAliasPath,
                    PageAlias = a.NodeAlias,
                    PageKeywords = a.DocumentPageKeyWords,
                    ExcludedFromSearch = a.DocumentSearchExcluded,
                    
                    
                }).FirstOrDefault();
        }

        public TravelTypeLandingModel GetLanding(string path)
        {
            return TravelTypeLandingProvider.GetTravelTypeLandings().Path(path).OnCurrentSite().OrderBy("NodeOrder")
                .Select(a => ModelFactory.CreateTravelLandingModel(a)
                ).FirstOrDefault();
        }

        public IEnumerable<TravelTypeDetailSummaryModel> GetTravelTypeDetails(string path)
        {
            return TravelTypeDetailProvider.GetTravelTypeDetails().Path(path, PathTypeEnum.Children).OnCurrentSite()
                .OrderBy("NodeOrder")
                .Select(a => ModelFactory.CreateTravelTypeDetailSummaryModel(a)
              ).ToList();
        }
    }
}
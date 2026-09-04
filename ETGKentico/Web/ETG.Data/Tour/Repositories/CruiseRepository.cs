using Castle.Core.Internal;
using CMS.DataEngine;
using ETG.Core.Kentico;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Tour.Factories;
using ETG.Data.Tour.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ETG.Data.Tour.Repositories
{
    public class CruiseRepository : ITourProductRepository
    {
        private readonly ISiteContext _siteContext;
        private readonly ITourModelFactory _tourModelFactory;
        public CruiseRepository(ISiteContext siteContext, ITourModelFactory tourModelFactory)
        {
            _siteContext = siteContext;
            _tourModelFactory = tourModelFactory;
        }


        public TourModel GetTour(string path)
        {
            return CruiseProvider.GetCruises().Path(path).OnCurrentSite().Select(_tourModelFactory.CreateTourModel).FirstOrDefault();
        }

        public List<TourSummaryInfoModel> GetToursByTourCodes(List<string> tourcodes)
        {
            if (tourcodes.IsNullOrEmpty())
            {
                return null;
            }

            return CruiseProvider.GetCruises().WhereIn("TourCode", tourcodes).OnCurrentSite().Select(_tourModelFactory.CreateSummaryInfoModel).ToList();
        }



        public TourModel GetTourbyPageAlias(string alias)
        {
            return CruiseProvider.GetCruises().WhereLike("NodeAlias", alias).OnCurrentSite().Select(_tourModelFactory.CreateTourModel).FirstOrDefault();
        }



        public TourModel GetTourByTourCode(string tourcode)
        {
            return CruiseProvider.GetCruises().WhereLike("TourCode", tourcode).OnCurrentSite().Select(_tourModelFactory.CreateTourModel).FirstOrDefault();
        }

        public List<TourSummaryInfoModel> GetToursByExperience(Guid ExperienceGuid, int topN)
        {
            if (ExperienceGuid == Guid.Empty)
            {
                return null;
            }
            return CruiseProvider.GetCruises().WhereLike("TourExperiences", $"%{ExperienceGuid}%").OnCurrentSite().TopN(topN).Select(_tourModelFactory.CreateSummaryInfoModel).ToList();

        }

        public List<TourSummaryInfoModel> GetToursByDestination(Guid DestinationGuid, int topN)
        {
            if (DestinationGuid == Guid.Empty)
            {
                return null;
            }
            return CruiseProvider.GetCruises().WhereEquals("TourPrimaryCountry", DestinationGuid).OnCurrentSite().TopN(topN).Select(_tourModelFactory.CreateSummaryInfoModel).ToList();

        }

        public List<TourSummaryInfoModel> GetTours(int topN)
        {
            return CruiseProvider.GetCruises().OnCurrentSite().TopN(topN).Select(_tourModelFactory.CreateSummaryInfoModel).ToList();
        }

        public List<TourSummaryInfoModel> GetToursByDestinations(List<Guid> destinationGuids, int topN)
        {
            if (destinationGuids.IsNullOrEmpty())
            {
                return null;
            }

            var whereAll = new WhereCondition();

            for (var i = 0; i < destinationGuids.Count; i++)
            {
                if (i > 0)
                {
                    whereAll = whereAll.Or();
                }
                var guid = destinationGuids[i];
                whereAll = whereAll.Where(new WhereCondition("TourPrimaryCountry", QueryOperator.Like, $"%{guid}%"));

            }
            return CruiseProvider.GetCruises().Where(whereAll).OnCurrentSite().TopN(topN).Select(_tourModelFactory.CreateSummaryInfoModel).ToList();
        }
    }
}

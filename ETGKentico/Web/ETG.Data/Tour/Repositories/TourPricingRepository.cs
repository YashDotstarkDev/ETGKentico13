using System;
using Castle.Core.Internal;
using CMS.DataEngine;
using CMS.DocumentEngine;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Factories;
using ETG.Data.Tour.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETG.Core.PageTypes;

namespace ETG.Data.Tour.Repositories
{
    public class TourPricingRepository : ITourPricingRepository
    {
        public List<PricingModel> GetTourPricings(string aliasPath)
        {
            return TourPricingProvider.GetTourPricings()
                .OnCurrentSite()
                .Path(aliasPath, PathTypeEnum.Section)
                .OrderBy( nameof(TourPricing.NodeOrder))
                .Select(ModelFactory.CreatePricingModel)
                .ToList();
        }

        public List<PricingModel> GetTourPricings(List<string> aliasPaths)
        {
            if (aliasPaths.IsNullOrEmpty())
            {
                return null;
            }

            var whereCondition = new StringBuilder();

            whereCondition.Append("(");
            for (var i = 0; i < aliasPaths.Count; i++)
            {
                if (i > 0)
                {
                    whereCondition.Append(" or ");
                }

                whereCondition.Append($"NodeAliasPath LIKE '{aliasPaths[i]}/%'");
            }
            whereCondition.AppendLine(")");

            return TourPricingProvider.GetTourPricings().Where(new WhereCondition(whereCondition.ToString())).OnCurrentSite().Select(
                ModelFactory.CreatePricingModel).ToList();
        }

        public List<PricingModel> GetTourPricings(List<Guid> tourGuids)
        {
            return TourPricingProvider.GetTourPricings().WhereIn( nameof(TourPricing.TourPricingTourGuid), tourGuids).OnCurrentSite().Select(
                ModelFactory.CreatePricingModel).ToList();

        }


    }
}
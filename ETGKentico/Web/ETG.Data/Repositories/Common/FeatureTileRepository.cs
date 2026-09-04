using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using Devotion.Web.Base.Extensions;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Models.Common;

namespace ETG.Data.Repositories.Common
{
    public class FeatureTileRepository : IFeatureTileRepository
    {
        public List<FeatureTileModel> Get(string url, string path = "")
        {
            return FeatureTileProvider.GetFeatureTiles()
                .Path(path.BeginWithSlash(), PathTypeEnum.Children)
                .OrderBy("NodeOrder")
                .OnCurrentSite()
                .Select(s => new FeatureTileModel
                {
                    LargeText = s.FeatureTileLargeText,
                    SmallText = s.FeatureTileSmallText,
                    CTALabel = s.FeatureTileCTALabel,
                    CTAUrl = s.FeatureTileCTAUrl,
                    Image = s.FeatureTileImage,
                    IsOffer = s.FeatureTileIsOffer
                }).ToList();
        }
    }
}

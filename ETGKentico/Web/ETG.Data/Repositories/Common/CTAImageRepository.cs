using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using Devotion.Web.Base.Extensions;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Models.Common;

namespace ETG.Data.Repositories.Common
{
    public class CTAImageRepository : ICTAImageRepository
    {
        public List<CTAImageModel> GetCTAImages(string path)
        {
            return CTAImageProvider.GetCTAImages()
                .Path(path.BeginWithSlash(), PathTypeEnum.Children)
                .OrderBy("NodeOrder")
                .OnCurrentSite()
                .Select(s => new CTAImageModel
                {   
                    ImagePath = s.CTAImageImagePath,
                    Url = s.CTAImageUrl,
                    ImageAltText = s.CTAImageImageAltText
                }).ToList();
        }

        public List<ImageTileCtaModel> GetImageTileCTAs(string path)
        {
            return ImageTileCtaProvider.GetImageTileCtas()
                 .Path(path.BeginWithSlash(), PathTypeEnum.Children)
                 .OrderBy("NodeOrder")
                 .OnCurrentSite()
                 .Select(s => new ImageTileCtaModel
                 {
                     ImagePath = s.Image,
                     Url = s.CtaUrl,
                     Caption = s.Caption,
                     SubCaption = s.SubCaption,
                     CtaLabel = s.CtaLabel,
                 }).ToList();
        }
    }
}

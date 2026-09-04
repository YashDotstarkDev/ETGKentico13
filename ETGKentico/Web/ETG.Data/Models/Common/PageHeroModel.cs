using Devotion.Automapper.Common;
using ETG.Data.Models.PageTypes;
using System.Collections.Generic;

namespace ETG.Data.Models.Common
{
    public class PageHeroModel : IDataModel
    {
        public string Heading { get; set; }
        public string HeadingSummary { get; set; }
        public string HeroImage { get; set; }
        public string HeroForegroundImage { get; set; }
        public bool DisableOverlay { get; set; }
        public string HeroImageAltText { get; set; }
        public string HeroIconImage { get; set; }
        public string HeroIconSvg { get; set; }
        public string HeroCaption { get; set; }
        public string CampaignTitle { get; set; }
        public bool LargeHeading { get; set; }
        public List<ImageModel> GalleryImages { get; set; }
        public bool HideShareButton { get; set; }
        public ShareLinksModel ShareLinks { get; set; }
        public string PageUrl { get; set; }
        public string HeroContactUsCtaPath { get; set; }
        public string HeroPhone { get; set; }
    }
}
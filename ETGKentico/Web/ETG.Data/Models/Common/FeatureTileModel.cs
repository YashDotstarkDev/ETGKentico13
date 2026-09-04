using Devotion.Automapper.Common;

namespace ETG.Data.Models.Common
{
    public class FeatureTileModel : IDataModel
    {
        public string LargeText { get; set; }
        public string SmallText { get; set; }
        public bool IsOffer { get; set; }

        public string Image { get; set; }
        public string CTALabel { get; set; }
        public string CTAUrl { get; set; }

    }
}
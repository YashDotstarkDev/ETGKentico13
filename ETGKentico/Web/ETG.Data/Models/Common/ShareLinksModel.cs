using Devotion.Automapper.Common;

namespace ETG.Data.Models.Common
{
    public class ShareLinksModel : IDataModel
    {
        public string FacebookShareUrl { get; set; }
        public string TwitterShareUrl { get; set; }
        public string InstagramShareUrl { get; set; }
        public string PinterestShareUrl { get; set; }
        public string LinkedInShareUrl { get; set; }
        public string CopyUrl { get; set; }
    }
}

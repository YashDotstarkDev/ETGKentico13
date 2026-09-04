using Devotion.Automapper.Common;

namespace ETG.Data.Models.Common
{
    public class CTAImageModel : IDataModel
    {
        public string ImagePath { get; set; }
        public string ImageAltText { get; set; }
        public string Url { get; set; }
    }
}
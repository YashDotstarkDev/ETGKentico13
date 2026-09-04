using Devotion.Automapper.Common;

namespace ETG.Data.Models.Common
{
    public class ImageModel : IDataModel
    {
        public string ImagePath { get; set; }
        public string ImageAltText { get; set; }
        public string ImageCaption { get; set; }
    }
}

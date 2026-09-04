using Devotion.Automapper.Common;

namespace ETG.Data.Models.Common
{
    public class ImageTileCtaModel : IDataModel
    {
        public string Caption { get; set; }
        public string SubCaption { get; set; }
        public string ImagePath { get; set; }
        public string Url { get; set; }

        public string CtaLabel { get; set; }
    }
}
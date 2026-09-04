using Devotion.Automapper.Common;

namespace ETG.Data.Models.PageTypes
{
    public class ThemedPackageModel : IDataModel
    {
        public string Heading { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string CtaLabel { get; set; }
        public string CtaUrl { get; set; }
    }
}
using Devotion.Automapper.Common;

namespace ETG.Data.Models.Common
{
    public class CTAIconModel : IDataModel
    {
        public string Label { get; set; }
        public string Url { get; set; }

        public string SvgIcon { get; set; }
        public string IconClass { get; set; }
    }
}
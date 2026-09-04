using ETG.Web.Models.Base;
using ETG.Web.Models.Common;
using ETG.Web.Models.PageTypes;

namespace ETG.Web.Models.Common
{
    public class ThemedPackageViewModel : BasePageViewModel, IViewModel
    {
        public string Heading { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string CtaLabel { get; set; }
        public string CtaUrl { get; set; }
    }
}
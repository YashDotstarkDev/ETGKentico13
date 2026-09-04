using System.Collections.Generic;
using ETG.Web.Models.Base;

namespace ETG.Web.Models.Common
{
    public class ThemedPackagesListingViewModel:  IViewModel
    {
        public string Heading { get; set; }
        public List<ThemedPackageViewModel> ThemedPackages { get; set; }
    }
}
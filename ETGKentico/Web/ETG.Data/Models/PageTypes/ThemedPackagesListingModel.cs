using System.Collections.Generic;
using Devotion.Automapper.Common;

namespace ETG.Data.Models.PageTypes
{
    public class ThemedPackagesListingModel: IDataModel
    {
        public string Heading { get; set; }
        public List<ThemedPackageModel> ThemedPackages { get; set; }
    }
}
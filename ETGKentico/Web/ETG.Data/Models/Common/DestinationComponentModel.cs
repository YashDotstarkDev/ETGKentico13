using Devotion.Automapper.Common;
using ETG.Data.Destination.Models;
using System.Collections.Generic;

namespace ETG.Data.Models.Common
{
    public class DestinationComponentModel : IDataModel
    {
        public string Title { get; set; }
        public List<DestinationSummaryModel> Destinations { get; set; }
    }
}

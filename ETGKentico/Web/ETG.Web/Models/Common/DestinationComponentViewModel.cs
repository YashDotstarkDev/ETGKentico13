using ETG.Web.Destination.Models;
using System.Collections.Generic;

namespace ETG.Web.Models.Common
{
    public class DestinationComponentViewModel : IViewModel
    {
        public string Title { get; set; }
        public List<DestinationSummaryViewModel> Destinations { get; set; }
    }
}

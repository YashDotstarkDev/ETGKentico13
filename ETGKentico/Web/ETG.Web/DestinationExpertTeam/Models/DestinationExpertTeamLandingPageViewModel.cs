using ETG.Web.DestinationExpertTeam.Models;
using ETG.Web.Models.Base;
using System.Collections.Generic;
using ETG.Web.Models;
using ETG.Web.Models.PageTypes;

namespace ETG.Web.DestinationExpertTeam.Models
{
    public class DestinationExpertTeamLandingPageViewModel : BasePageViewModel, IViewModel
    {
        public PageItemViewModel Page { get; set; }
        public List<DestinationExpertTeamSummaryViewModel> DestinationExperts { get; set; }
    }
}

using ETG.Data.Models.Base;
using System.Collections.Generic;
using Devotion.Automapper.Common;
using ETG.Data.Models.PageTypes;

namespace ETG.Data.DestinationExpertTeam.Models
{
    public class DestinationExpertTeamLandingPageModel : BasePageModel, IDataModel
    {
        public PageItemModel Page { get; set; }
        public List<DestinationExpertTeamSummaryModel> DestinationExperts { get; set; }
    }
}

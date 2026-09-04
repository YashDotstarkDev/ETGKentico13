using Castle.Core.Internal;
using ETG.Data.Models.Base;
using System.Collections.Generic;

namespace ETG.Data.DestinationExpertTeam.Models
{
    public class DestinationExpertTeamModel : PageNodeModel
    {
        public DestinationExpertTeamSummaryModel SummaryInfo { get; set; }

        public string ObjectiveStatement { get; set; }
        public string Profile { get; set; }
        public string GoalsInText { get; set; }
        public string FeatureTourCodes { get; set; }

        public string FeatureCruiseCodes { get; set; }

    }
}

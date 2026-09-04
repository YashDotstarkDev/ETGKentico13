using ETG.Web.DestinationExpertTeam.Models;
using System.Collections.Generic;

namespace ETG.Web.Models.Common
{
    public class GenericSideContactViewModel : IViewModel
    {
        public ContactViewModel GeneralContact { get; set; }
        public List<DestinationExpertTeamSummaryViewModel> DestinationExpertContacts { get; set; }

    }
}

using Devotion.Automapper.Common;
using ETG.Data.DestinationExpertTeam.Models;
using System.Collections.Generic;

namespace ETG.Data.Models.Common
{
    public class GenericSideContactModel : IDataModel
    {
        public ContactModel GeneralContact { get; set; }
        public List<DestinationExpertTeamSummaryModel> DestinationExpertContacts { get; set; }

    }
}

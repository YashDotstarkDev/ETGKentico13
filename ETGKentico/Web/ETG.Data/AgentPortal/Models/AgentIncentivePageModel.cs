using Devotion.Automapper.Common;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;

namespace ETG.Data.AgentPortal.Models
{
    public class AgentIncentivePageModel : BasePageModel, IDataModel
    {
        public AgentIncentiveModel Page { get; set; }
        public PageHeroModel Hero { get; set; }
    }
}

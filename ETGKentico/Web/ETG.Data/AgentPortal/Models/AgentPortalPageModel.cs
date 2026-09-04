using System.Collections.Generic;
using Devotion.Automapper.Common;
using ETG.Data.Brochure.Models;
using ETG.Data.Models.Base;

namespace ETG.Data.AgentPortal.Models
{
    public class AgentPortalPageModel : BasePageModel, IDataModel
    {
        public AgentPortalMainModel Page { get; set; }
        public List<AgentToolkitModel> AgentToolkits { get; set; }
        public List<AgentIncentiveModel> AgentIncentives { get; set; }
        public List<AgentWebinarModel> AgentWebinars { get; set; }
        public List<BrochureModel> Brochures { get; set; }
    }
}

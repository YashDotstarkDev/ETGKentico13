using ETG.Web.Models;
using System;

namespace ETG.Web.AgentPortal.Models
{
    public class AgentWebinarViewModel : IViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string RecordingUrl { get; set; }
        public DateTime ComingSoonDate { get; set; }

    }
}

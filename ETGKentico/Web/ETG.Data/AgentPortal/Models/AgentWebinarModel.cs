using System;
using Devotion.Automapper.Common;

namespace ETG.Data.AgentPortal.Models
{
    public class AgentWebinarModel : IDataModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string RecordingUrl { get; set; }
        public DateTime ComingSoonDate { get; set; }

    }
}

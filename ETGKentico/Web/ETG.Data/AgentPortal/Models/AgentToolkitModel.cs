using Devotion.Automapper.Common;

namespace ETG.Data.AgentPortal.Models
{
    public class AgentToolkitModel : IDataModel
    {
        public  string Name { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
    }
}

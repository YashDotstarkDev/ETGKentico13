using Devotion.Automapper.Common;

namespace ETG.Data.Models.Common
{
    public class SimpleLinkModel : IDataModel
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public string Url { get; set; }

        public bool Active { get; set; }

        public string IconClass { get; set; }
    }
}

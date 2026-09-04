using Devotion.Automapper.Common;

namespace ETG.Data.Models.Common
{
    public class NameValuePathModel : IDataModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public int NodeOrder { get; set; }
    }
}
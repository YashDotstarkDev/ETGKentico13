namespace ETG.Web.Models.Common
{
    public class NameValuePathViewModel : IViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public int NodeOrder { get; set; }
    }
}
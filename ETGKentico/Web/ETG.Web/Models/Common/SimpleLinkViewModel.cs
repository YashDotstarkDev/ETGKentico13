namespace ETG.Web.Models.Common
{
    public class SimpleLinkViewModel : IViewModel
    {
        public string Id { get; set; }
        public string Label { get; set; }

        public string Url { get; set; }

        public bool Active { get; set; }

        public string IconClass { get; set; }
    }
}

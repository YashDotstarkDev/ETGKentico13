namespace ETG.Web.Models.Partial
{
    public class SiteSelectorPopupModel
    {
        public string Title { get; set; }
        public string DomainNz { get; set; }
        public string CtaTextNz { get; set; }
        public string CtaTextAu { get; set; }
        public string GetCountryEndpoint { get; set; }
        public bool IsNz { get; set; } = false;
    }
}
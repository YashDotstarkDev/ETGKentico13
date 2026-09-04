using CMS.SiteProvider;

namespace ETG.Core.Kentico
{
    using static CMS.SiteProvider.SiteContext;

    public class KenticoSiteContext : ISiteContext
    {
        public string SiteName => CurrentSiteName;

        public int SiteId => CurrentSiteID;

        public SiteInfo Site => CurrentSite;

        public string CurrentCultureCode => "en-AU";
    }
}

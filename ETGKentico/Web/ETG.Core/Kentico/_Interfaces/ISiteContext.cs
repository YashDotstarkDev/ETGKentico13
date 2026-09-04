using CMS.Localization;
using CMS.SiteProvider;

namespace ETG.Core.Kentico
{
    public interface ISiteContext
    {
        string SiteName { get; }
        int SiteId { get; }
        SiteInfo Site { get; }
        string CurrentCultureCode { get; }
    }
}

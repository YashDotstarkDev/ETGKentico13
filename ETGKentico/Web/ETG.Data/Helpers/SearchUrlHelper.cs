using Castle.Core.Internal;
using CMS.Helpers;

namespace ETG.Data.Helpers
{
    public static class SearchUrlHelper
    {
        public static string GetExperienceFilterUrl(string tourIndex, string experience)
        {
            if (experience.IsNullOrEmpty())
            {
                return $"/search";
            }
            return $"/search?{tourIndex}%5BrefinementList%5D%5Bexperiences%5D%5B0%5D={experience}";
        }

        public static string GetDestinationFilterUrl(string tourIndex, string destinations, string tab = null)
        {
            var returnUrl = $"/search?{tourIndex}%5BrefinementList%5D%5Bdestination%5D%5B0%5D={destinations}";
            if (!string.IsNullOrEmpty(tab))
            {
                returnUrl = URLHelper.AddParameterToUrl(returnUrl, "tab", tab);
            }
            return returnUrl;
        }
    }
}

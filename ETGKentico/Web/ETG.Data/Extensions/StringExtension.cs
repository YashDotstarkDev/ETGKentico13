using CMS.Helpers;

namespace ETG.Data.Extensions
{
    public static class StringExtension
    {
        public static string StripHtml(this string str)
        {
            return HTMLHelper.StripTags(str).Replace("\r\n", string.Empty).Replace("\n", string.Empty);
        }
    }
}

namespace Devotion.Cache.Common
{
    public static class PathUtil
    {
        public static string TrimAliasPath(string aliasPath)
        {
            return string.IsNullOrEmpty(aliasPath) ? string.Empty : aliasPath.TrimEnd('%').TrimEnd('/');
        }
    }
}

namespace Devotion.Web.Base.Providers
{
    public interface IHtmlSanitiserProvider
    {
        string RemoveUnwantedTags(string input, string[] acceptTags = null);
    }
}

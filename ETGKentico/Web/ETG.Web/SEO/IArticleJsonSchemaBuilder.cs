using ETG.Web.Article.Models;

namespace ETG.Web.SEO
{
    public interface IArticleJsonSchemaBuilder
    {
        string BuildJsonSchema(ArticleViewModel article, string httpDomain);
    }
}

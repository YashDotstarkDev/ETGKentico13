using Devotion.Data;
using ETG.Data.Article.Models;

namespace ETG.Data.Article.Repositories
{
    public interface IArticlePageRepository : IRepository<ArticlePageModel>
    {
        ArticleLandingPageModel GetLandingPage(string currentUrl, string currentCategory);
    }
}

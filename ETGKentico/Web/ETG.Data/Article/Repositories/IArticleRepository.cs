using ETG.Data.Article.Models;
using ETG.Data.Models.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ETG.Data.Article.Repositories
{
    public interface IArticleRepository
    {
        ArticleModel GetArticle(string path);
        ArticleModel GetArticleByAlias(string alias);

        List<ArticleModel> GetRelatedArticlesByExperience(Guid experienceNodeGuid);
        List<ArticleModel> GetRelatedArticlesByDestination(Guid destinationNodeGuid);
        List<ArticleModel> GetRelatedArticlesByTravelType(Guid travelTypeNodeGuid);

        Task<SearchResults<ArticleModel>> GetArticlesAsync(ArticleLandingFilter filter);
        List<ArticleModel> GetLatestArticles(int topN = 0);
        List<ArticleModel> GetRelatedArticlesByDestinations(List<Guid> destinationGuids);

        List<ArticleModel> GetLatestArticlesByCategory(string categoryGuid, int topN = 0);
        int GetTotalCount(string category);
    }
}

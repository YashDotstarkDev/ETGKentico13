using ETG.Data.Article.Models;
using System;
using System.Collections.Generic;

namespace ETG.Data.Article.Services
{
    public interface IArticleService
    {
        List<ArticleModel> GetLatestArticles(int topN);
        List<ArticleModel> GetRelatedArticles(ArticleModel article);
        List<ArticleModel> GetRelatedArticlesByExperience(Guid experienceNodeGuid);
        List<ArticleModel> GetRelatedArticlesByDestination(Guid destinationNodeGuid);
        List<ArticleModel> GetRelatedArticlesByDestinations(List<Guid> destinationGuids);

        List<ArticleModel> GetRelatedArticlesByTravelType(Guid travelTypeNodeGuidNodeGuid);

        List<ArticleModel> GetLatestArticlesByCategory(int topN, string categoryGuid);
    }
}

using Castle.Core.Internal;
using CMS.DataEngine;
using CMS.DocumentEngine;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Article.Models;
using ETG.Data.Article.Repositories;
using ETG.Data.Cache;
using ETG.Data.Repositories.Modules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ETG.Data.Article.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly ICacheService _cacheService;
        public ArticleService(IArticleRepository articleRepository, IArticleCategoryRepository articleCategoryRepository, ICacheService cacheService)
        {
            _articleRepository = articleRepository;
            _articleCategoryRepository = articleCategoryRepository;
            _cacheService = cacheService;
        }
        private WhereCondition GetMatchAllWhereCondition(ArticleModel article)
        {
            WhereCondition mainWhere = null;
            if (!article.CategoryGuids.IsNullOrEmpty())
            {
                var whereString = string.Join(" and ", article.CategoryGuids.Split(';').Select(a => $"ArticleCategories LIKE '%{a}%'"));

                mainWhere = new WhereCondition(whereString);
            }

            if (!article.DestinationGuids.IsNullOrEmpty())
            {
                var whereString = string.Join(" and ", article.DestinationGuids.Split(';').Select(a => $"ArticleDestinations LIKE '%{a}%'"));

                if (mainWhere == null)
                {
                    mainWhere = new WhereCondition(whereString);
                }
                else
                {
                    mainWhere = mainWhere.Where(whereString);
                }
            }

            return mainWhere;
        }

        private WhereCondition GetMatchDestinationAndCategoryWhereCondition(ArticleModel article, List<ArticleModel> currentList)
        {
            WhereCondition mainWhere = null;
            WhereCondition condition1 = null;
            WhereCondition condition2 = null;
            if (!article.CategoryGuids.IsNullOrEmpty())
            {
                var whereString = string.Join(" or ", article.CategoryGuids.Split(';').Select(a => $"ArticleCategories LIKE '%{a}%'"));

                condition1 = new WhereCondition($"({whereString})");
            }

            if (!article.DestinationGuids.IsNullOrEmpty())
            {
                var whereString = string.Join(" or ", article.DestinationGuids.Split(';').Select(a => $"ArticleDestinations LIKE '%{a}%'"));
                condition2 = new WhereCondition($"({whereString})");
            }

            if (condition1 != null && condition2 != null)
            {

                mainWhere = (new WhereCondition()).WhereNotIn("DocumentID", currentList.Select(a => a.DocumentID).ToList());
                if (condition1 != null)
                {
                    mainWhere = mainWhere.And(condition1);
                }

                if (condition2 != null)
                {
                    mainWhere = mainWhere.And(condition2);
                }

                return mainWhere;
            }

            return null;
        }

        private WhereCondition GetMatchDestinationWhereCondition(ArticleModel article, List<ArticleModel> currentList)
        {
            if (article.DestinationGuids.IsNullOrEmpty())
            {
                return null;
            }
            var whereString = string.Join(" or ", article.DestinationGuids.Split(';').Select(a => $"ArticleDestinations LIKE '%{a}%'"));

            var whereCondition = new WhereCondition($"({whereString})");

            return whereCondition.And(new WhereCondition().WhereNotIn("DocumentID", currentList.Select(a => a.DocumentID).ToList()));

        }


        private WhereCondition GetMatchCategoryWhereCondition(ArticleModel article, List<ArticleModel> currentList)
        {

            if (article.CategoryGuids.IsNullOrEmpty())
            {
                return null;
            }
            var whereString = string.Join(" or ", article.CategoryGuids.Split(';').Select(a => $"ArticleCategories LIKE '%{a}%'"));

            var whereCondition = new WhereCondition($"({whereString})");
            return whereCondition.And((new WhereCondition()).WhereNotIn("DocumentID", currentList.Select(a => a.DocumentID).ToList()));

        }

        private ArticleModel CreateModel(Core.PageTypes.Article a)
        {
            return new ArticleModel
            {
                DocumentID = a.DocumentID,
                PageAliasPath = a.NodeAliasPath,
                PageAlias = a.NodeAlias,
                Title = a.ArticleTitle,
                HeroImage = a.ArticleHeroImage,
                PublishDate = a.ArticleDate,
                AuthorGuid = a.ArticleAuthor,
                CategoryGuids = a.ArticleCategories,
                DestinationGuids = a.ArticleDestinations,
                Url = $"/articles/{a.NodeAlias}"
            };
        }

        private DocumentQuery<Core.PageTypes.Article> GetQuery(ArticleModel article)
        {
            return ArticleProvider.GetArticles().OnCurrentSite().WhereNotEquals("DocumentID", article.DocumentID);
        }
        private List<ArticleModel> GetMatchingArticles(ArticleModel article)
        {

            var mainWhere = GetMatchAllWhereCondition(article);
            List<ArticleModel> related = new List<ArticleModel>();
            if (mainWhere != null)
            {

                related = GetQuery(article).Where(mainWhere).OrderByDescending("ArticleDate").Select(CreateModel).Take(2).ToList();

                if (related.Count == 2)
                {
                    return related;
                }
            }

            mainWhere = GetMatchDestinationAndCategoryWhereCondition(article, related);

            if (mainWhere != null)
            {
                var match = GetQuery(article).Where(mainWhere).OrderByDescending("ArticleDate").Take(2).Select(CreateModel).ToList();

                if (match.Count > 0)
                {
                    related.AddRange(match);
                    if (related.Count >= 2)
                    {
                        return related.Take(2).ToList();
                    }
                }

            }

            mainWhere = GetMatchDestinationWhereCondition(article, related);

            if (mainWhere != null)
            {
                var match = GetQuery(article).Where(mainWhere).OrderByDescending("ArticleDate").Select(CreateModel).Take(2).ToList();

                if (match.Count > 0)
                {
                    related.AddRange(match);
                    if (related.Count >= 2)
                    {
                        return related.Take(2).ToList();
                    }
                }

            }

            mainWhere = GetMatchCategoryWhereCondition(article, related);

            if (mainWhere != null)
            {
                var match = GetQuery(article).Where(mainWhere).OrderByDescending("ArticleDate").Select(CreateModel).Take(2).ToList();

                if (match.Count > 0)
                {
                    related.AddRange(match);
                    if (related.Count >= 2)
                    {
                        return related.Take(2).ToList();
                    }
                }

            }


            return related;
        }
        public List<ArticleModel> GetRelatedArticles(ArticleModel article)
        {
            var relatedArticles = GetMatchingArticles(article);

            if (!relatedArticles.IsNullOrEmpty())
            {
                for (var i = 0; i < relatedArticles.Count; i++)
                {

                    var categoryList = _articleCategoryRepository.GetArticleCategories(relatedArticles[i].CategoryGuids);

                    if (!categoryList.IsNullOrEmpty())
                    {
                        relatedArticles[i].Categories = categoryList.Select(a => new KeyValuePair<Guid, string>(a.ItemGuid, a.Name)).ToList();
                    }
                }
            }

            return relatedArticles;
        }

        public List<ArticleModel> GetRelatedArticlesByExperience(Guid experienceNodeGuid)
        {
            return _cacheService.GetDocumentDependentOnAll(() => _articleRepository.GetRelatedArticlesByExperience(experienceNodeGuid), $"GetRelatedArticlesByExperience{experienceNodeGuid}", Core.PageTypes.Article.CLASS_NAME);
        }

        public List<ArticleModel> GetRelatedArticlesByDestination(Guid destinationNodeGuid)
        {
            return _cacheService.GetDocumentDependentOnAll(() => _articleRepository.GetRelatedArticlesByDestination(destinationNodeGuid), $"GetRelatedArticlesByDestination{destinationNodeGuid}", Core.PageTypes.Article.CLASS_NAME);
        }
        
        public List<ArticleModel> GetRelatedArticlesByTravelType(Guid travelTypeNodeGuidNodeGuid)
        {
            return _cacheService.GetDocumentDependentOnAll(() =>
                _articleRepository.GetRelatedArticlesByTravelType(travelTypeNodeGuidNodeGuid)
                , $"GetRelatedArticlesByTravelType{travelTypeNodeGuidNodeGuid}", Core.PageTypes.Article.CLASS_NAME);
        }

        public List<ArticleModel> GetLatestArticles(int topN)
        {
            return _cacheService.GetDocumentDependentOnAll(() => _articleRepository.GetLatestArticles(topN), $"GetLatestArticles{topN}", Core.PageTypes.Article.CLASS_NAME);
        }

        public List<ArticleModel> GetLatestArticlesByCategory(int topN, string categoryGuid)
        {
            return _cacheService.GetDocumentDependentOnAll(() => _articleRepository.GetLatestArticlesByCategory(categoryGuid, topN), $"GetLatestArticles{categoryGuid}{topN}", Core.PageTypes.Article.CLASS_NAME);
        }

        public List<ArticleModel> GetRelatedArticlesByDestinations(List<Guid> destinationGuids)
        {
            if (destinationGuids.IsNullOrEmpty())
            {
                return null;
            }

            return _cacheService.GetDocumentDependentOnAll(() => _articleRepository.GetRelatedArticlesByDestinations(destinationGuids), $"GetRelatedArticlesByDestinations{destinationGuids[0]}", Core.PageTypes.Article.CLASS_NAME);
        }
    }
}

using Castle.Core.Internal;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Article.Models;
using ETG.Data.Models.Base;
using ETG.Data.Repositories.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.DataEngine;
using CMS.EventLog;
using Devotion.Web.Base.Extensions;
using ETG.Core.Extensions;
using ETG.Data.Destination.Services;
using CMS.Helpers;
using CMS.SiteProvider;
using CMS.Localization;

namespace ETG.Data.Article.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IArticleAuthorRepository _articleAuthorRepository;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IDestinationService _destinationService;
        public ArticleRepository(IArticleAuthorRepository articleAuthorRepository,
            IArticleCategoryRepository articleCategoryRepository,
            IDestinationService destinationService)
        {
            _articleAuthorRepository = articleAuthorRepository;
            _articleCategoryRepository = articleCategoryRepository;
            _destinationService = destinationService;
        }

        private ArticleModel CreateModel(Core.PageTypes.Article a)
        {
            var destination = DestinationProvider.GetDestination(ValidationHelper.GetGuid(a.ArticleDestinations, Guid.Empty),
                LocalizationContext.CurrentCulture.CultureCode, SiteContext.CurrentSiteName).FirstOrDefault();

            var article = new ArticleModel
            {
                DocumentID = a.DocumentID,
                PageTitle = a.DocumentPageTitle,
                PageDescription = a.DocumentPageDescription,
                PageAliasPath = a.NodeAliasPath,
                PageAlias = a.NodeAlias,
                PageKeywords = a.DocumentPageKeyWords,
                ShareTitle = a.ArticleTitle,
                ShareDescription = a.DocumentPageDescription,
                ShareImage = a.ArticleHeroImage,
                ExcludedFromSearch = a.DocumentSearchExcluded,
                Title = a.ArticleTitle,
                Summary = a.ArticleSummary,
                HeroImage = a.ArticleHeroImage.Imgixify(),
                HeroImageAltText = a.ArticleHeroImageAltTag,
                PublishDate = a.ArticleDate,
                ModifiedDate = a.DocumentModifiedWhen,
                AuthorGuid = a.ArticleAuthor,
                CategoryGuids = a.ArticleCategories,
                DestinationGuids = a.ArticleDestinations,
                Url = $"/articles/{a.NodeAlias}",
                DocumentSearchExcluded = a.DocumentSearchExcluded,
                FirstDestinationName = destination?.DestinationName,
            };

            var categoryList = _articleCategoryRepository.GetArticleCategories(a.ArticleCategories);

            if (categoryList != null)
            {
                article.Categories = categoryList.Select(c=>new KeyValuePair<Guid, string>(c.ItemGuid, c.Name))
                    .ToList();

            }

            return article;
        }

        public ArticleModel GetArticle(string path)
        {
            if (path.IsNullOrEmpty())
            {
                return null;
            }
            var article = ArticleProvider.GetArticles().Path(path).OnCurrentSite().Select(CreateModel).FirstOrDefault();

            if (article == null)
            {
                return null;
            }

            return GetMoreArticleData(article);

        }
        public List<ArticleModel> GetRelatedArticlesByExperience(Guid experienceNodeGuid)
        {
            return ArticleProvider.GetArticles().WhereLike("ArticleExperiences", $"%{experienceNodeGuid}%").OnCurrentSite().OrderByDescending("ArticleDate")
                .Select(CreateModel).ToList();
        }

        public List<ArticleModel> GetRelatedArticlesByDestination(Guid destinationNodeGuid)
        {
            return ArticleProvider.GetArticles().WhereLike("ArticleDestinations", $"%{destinationNodeGuid}%").OnCurrentSite().OrderByDescending("ArticleDate").Take(3)
                .Select(CreateModel).ToList();
        }
        
        public List<ArticleModel> GetRelatedArticlesByTravelType(Guid travelTypeNodeGuid)
        {
            return ArticleProvider.GetArticles().WhereLike("ArticleTravelTypes", $"%{travelTypeNodeGuid}%").OnCurrentSite().OrderByDescending("ArticleDate")
                .Select(CreateModel).ToList();
        }

        public ArticleModel GetArticleByAlias(string alias)
        {
            if (alias.IsNullOrEmpty())
            {
                return null;
            }
            var article = ArticleProvider.GetArticles().WhereLike("NodeAlias", alias).OnCurrentSite().Select(CreateModel).FirstOrDefault();

            if (article == null)
            {
                return null;
            }

            return GetMoreArticleData(article);

        }

        private ArticleModel GetMoreArticleData(ArticleModel article)
        {
            if (article == null)
            {
                return null;
            }

            if (article.AuthorGuid != Guid.Empty)
            {
                var author = _articleAuthorRepository.GetAuthor(article.AuthorGuid);

                if (author != null)
                {
                    article.Author = new KeyValuePair<Guid, string>(author.ItemGuid, author.Name);
                }

            }

            if (!article.CategoryGuids.IsNullOrEmpty())
            {
                var categoryList = _articleCategoryRepository.GetArticleCategories(article.CategoryGuids);

                if (!categoryList.IsNullOrEmpty())
                {
                    article.Categories = categoryList.Select(a => new KeyValuePair<Guid, string>(a.ItemGuid, a.Name))
                        .ToList();
                }
            }

            if (!article.DestinationGuids.IsNullOrEmpty())
            {
                var guidList = article.DestinationGuids.Split(';').Where(a => a.IsGuid()).Select(a => a.ToGuid()).ToList();

                var destinationList = _destinationService.GetDestinations(guidList);

                if (!destinationList.IsNullOrEmpty())
                {
                    article.Destinations = destinationList.Select(a => new KeyValuePair<Guid, string>(a.DestinationGuid, a.Heading))
                        .ToList();
                }
            }

            return article;
        }

        public async  Task<SearchResults<ArticleModel>> GetArticlesAsync(ArticleLandingFilter filter)
        {
            var query = ArticleProvider.GetArticles().OnCurrentSite();
            if (filter != null)
            {
                if (!filter.ArticleCategoryId.IsNullOrEmpty() && !filter.ArticleCategoryId.Equals("all"))
                {
                    query = query.WhereLike("ArticleCategories", $"%{filter.ArticleCategoryId}%");
                }
            }

            var queryTotal = query.Select(a => GetMoreArticleData(CreateModel(a))).OrderBy(a=>a.DestinationsText);
            var result = new SearchResults<ArticleModel>();

            result.TotalCount = queryTotal.Count();
            if (filter != null && filter.ItemPerPage > 0)
            {
                int page = filter.PageNumber;

                if (page == 0)
                {
                    page = 1;
                }
                result.Results = queryTotal.Skip(((page - 1) * filter.ItemPerPage) + 1).Take(filter.ItemPerPage);
            }
            else
            {
                result.Results = queryTotal.ToList();
            }
            await Task.FromResult(0);
            return result;

        }

        public int GetTotalCount(string category)
        {

            

            var query = ArticleProvider.GetArticles().OnCurrentSite();
            if (!category.IsNullOrEmpty() && !category.Equals("all"))
            {
                var categoryObject = _articleCategoryRepository.GetArticleCategory(category.Replace(" ", "").Replace("-",""));

                if (categoryObject != null)
                {

                    query = query.WhereLike("ArticleCategories", $"%{categoryObject.ItemGuid}%");
                }
            }
            return query.Count();

        }

        public List<ArticleModel> GetLatestArticles(int topN = 0)
        {
            if (topN == 0)
            {
                return ArticleProvider.GetArticles().OnCurrentSite().OrderByDescending("ArticleDate")
                    .Select(CreateModel).ToList();
            }
            
            return ArticleProvider.GetArticles().OnCurrentSite().OrderByDescending("ArticleDate").TopN(topN)
                .Select(CreateModel).ToList();
        }

        public List<ArticleModel> GetLatestArticlesByCategory(string categoryGuid, int topN = 0)
        {
            if (topN == 0)
            {
                return ArticleProvider.GetArticles().OnCurrentSite().OrderByDescending("ArticleDate")
                    .WhereContains(nameof(Core.PageTypes.Article.ArticleCategories), categoryGuid)
                    .Select(CreateModel).ToList();
            }
            return ArticleProvider.GetArticles().OnCurrentSite().OrderByDescending("ArticleDate").TopN(topN)
                .WhereContains(nameof(Core.PageTypes.Article.ArticleCategories), categoryGuid)
                .Select(CreateModel).ToList();
        }

        public List<ArticleModel> GetRelatedArticlesByDestinations(List<Guid> destinationGuids)
        {

            var whereAll = new WhereCondition();
            
            for (var i=0; i< destinationGuids.Count;i++)
            {
                if (i > 0)
                {
                    whereAll = whereAll.Or();
                }
                var guid = destinationGuids[i];
                whereAll = whereAll.Where(new WhereCondition("ArticleDestinations", QueryOperator.Like, $"%{guid}%"));

            }


            return ArticleProvider.GetArticles().Where(whereAll).OnCurrentSite().OrderByDescending("ArticleDate")
                .Select(CreateModel).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using ETG.Data.Cache;
using ETG.Data.Models.Modules;
using ETG.Module.Classes.Info;
using ETG.Module.Classes.Providers;

namespace ETG.Data.Repositories.Modules
{
    public class ArticleAuthorRepository : IArticleAuthorRepository
    {
        private readonly ICacheService _cacheService;
        public ArticleAuthorRepository(ICacheService cacheService )
        {
            _cacheService = cacheService;
        }
        private List<AuthorModel> GetAllAuthorsInternal()
        {
            return ArticleAuthorInfoProvider.GetArticleAuthors().OrderBy("ArticleAuthorFullName").Select(
                a=> new AuthorModel
                {
                    ItemGuid = a.ArticleAuthorGuid,
                    Name = a.ArticleAuthorFullName
                }).ToList();
        }

        private List<AuthorModel> GetAllAuthors()
        {
            return _cacheService.GetAllObjectDependency(GetAllAuthorsInternal,
                "allarticleauthors", ArticleAuthorInfo.OBJECT_TYPE);
        }

        public AuthorModel GetAuthor(Guid guid)
        {
            return GetAllAuthors().FirstOrDefault(a => a.ItemGuid == guid);

        }

    }
}
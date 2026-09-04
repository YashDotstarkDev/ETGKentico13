using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Devotion.Web.Base.Extensions;
using ETG.Data.Cache;
using ETG.Data.Models.Modules;
using ETG.Module.Classes.Info;
using ETG.Module.Classes.Providers;

namespace ETG.Data.Repositories.Modules
{
    public class ArticleCategoryRepository : IArticleCategoryRepository
    {
        private readonly ICacheService _cacheService;
        public ArticleCategoryRepository(ICacheService cacheService )
        {
            _cacheService = cacheService;
        }
        private List<ArticleCategoryModel> GetAllCategoriesInternal()
        {
            return ArticleCategoryInfoProvider.GetArticleCategories().OrderBy("ArticleCategoryName").Select(
                a=> new ArticleCategoryModel
                {
                    ItemGuid = a.ArticleCategoryGuid,
                    Name = a.ArticleCategoryName,
                    Code = a.ArticleCategoryCodeName
                }).ToList();
        }

        public List<ArticleCategoryModel> GetAllCategories()
        {
            return _cacheService.GetAllObjectDependency(() => GetAllCategoriesInternal(),
                "allarticlecategories", ArticleCategoryInfo.OBJECT_TYPE);
        }

        
        public ArticleCategoryModel GetArticleCategory(Guid guid)
        {
            return GetAllCategories().FirstOrDefault(a => a.ItemGuid == guid);
        }

        public List<ArticleCategoryModel> GetArticleCategories(string guids)
        {
            var guidList = guids.Split(';').Where(a=>a.IsGuid()).Select(a=>a.ToGuid()).ToList();

            if (guidList.IsNullOrEmpty())
            {
                return null;
            }

            return GetAllCategories().Where(a => guidList.Contains(a.ItemGuid)).ToList();
        }

        public ArticleCategoryModel GetArticleCategory(string categoryCode)
        {
            return GetAllCategories().FirstOrDefault(a=>a.Code.ToLower() == categoryCode.ToLower());
        }
    }
}
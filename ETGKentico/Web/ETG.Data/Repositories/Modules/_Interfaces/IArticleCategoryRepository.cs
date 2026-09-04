using ETG.Data.Models.Modules;
using System;
using System.Collections.Generic;

namespace ETG.Data.Repositories.Modules
{
    public interface IArticleCategoryRepository
    {
        List<ArticleCategoryModel> GetAllCategories();
        ArticleCategoryModel GetArticleCategory(Guid guid);
        ArticleCategoryModel GetArticleCategory(string categoryCode);
        List<ArticleCategoryModel> GetArticleCategories(string guids);
    }
}
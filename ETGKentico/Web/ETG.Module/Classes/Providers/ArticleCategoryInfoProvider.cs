using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Classes.Info;

namespace ETG.Module.Classes.Providers
{
    /// <summary>
    /// Class providing <see cref="ArticleCategoryInfo"/> management.
    /// </summary>
    public partial class ArticleCategoryInfoProvider : AbstractInfoProvider<ArticleCategoryInfo, ArticleCategoryInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="ArticleCategoryInfoProvider"/>.
        /// </summary>
        public ArticleCategoryInfoProvider()
            : base(ArticleCategoryInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="ArticleCategoryInfo"/> objects.
        /// </summary>
        public static ObjectQuery<ArticleCategoryInfo> GetArticleCategories()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="ArticleCategoryInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="ArticleCategoryInfo"/> ID.</param>
        public static ArticleCategoryInfo GetArticleCategoryInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Returns <see cref="ArticleCategoryInfo"/> with specified name.
        /// </summary>
        /// <param name="name"><see cref="ArticleCategoryInfo"/> name.</param>
        public static ArticleCategoryInfo GetArticleCategoryInfo(string name)
        {
            return ProviderObject.GetInfoByCodeName(name);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="ArticleCategoryInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="ArticleCategoryInfo"/> to be set.</param>
        public static void SetArticleCategoryInfo(ArticleCategoryInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="ArticleCategoryInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="ArticleCategoryInfo"/> to be deleted.</param>
        public static void DeleteArticleCategoryInfo(ArticleCategoryInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="ArticleCategoryInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="ArticleCategoryInfo"/> ID.</param>
        public static void DeleteArticleCategoryInfo(int id)
        {
            ArticleCategoryInfo infoObj = GetArticleCategoryInfo(id);
            DeleteArticleCategoryInfo(infoObj);
        }
    }
}
using CMS.DataEngine;
using ETG.Module.Classes.Info;

namespace ETG.Module.Classes.Providers
{
    /// <summary>
    /// Class providing <see cref="ArticleAuthorInfo"/> management.
    /// </summary>
    public partial class ArticleAuthorInfoProvider : AbstractInfoProvider<ArticleAuthorInfo, ArticleAuthorInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="ArticleAuthorInfoProvider"/>.
        /// </summary>
        public ArticleAuthorInfoProvider()
            : base(ArticleAuthorInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="ArticleAuthorInfo"/> objects.
        /// </summary>
        public static ObjectQuery<ArticleAuthorInfo> GetArticleAuthors()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="ArticleAuthorInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="ArticleAuthorInfo"/> ID.</param>
        public static ArticleAuthorInfo GetArticleAuthorInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Returns <see cref="ArticleAuthorInfo"/> with specified name.
        /// </summary>
        /// <param name="name"><see cref="ArticleAuthorInfo"/> name.</param>
        public static ArticleAuthorInfo GetArticleAuthorInfo(string name)
        {
            return ProviderObject.GetInfoByCodeName(name);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="ArticleAuthorInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="ArticleAuthorInfo"/> to be set.</param>
        public static void SetArticleAuthorInfo(ArticleAuthorInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="ArticleAuthorInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="ArticleAuthorInfo"/> to be deleted.</param>
        public static void DeleteArticleAuthorInfo(ArticleAuthorInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="ArticleAuthorInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="ArticleAuthorInfo"/> ID.</param>
        public static void DeleteArticleAuthorInfo(int id)
        {
            ArticleAuthorInfo infoObj = GetArticleAuthorInfo(id);
            DeleteArticleAuthorInfo(infoObj);
        }
    }
}
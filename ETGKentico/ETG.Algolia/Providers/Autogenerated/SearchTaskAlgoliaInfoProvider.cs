using ETG.Algolia.Classes;
using CMS.DataEngine;

namespace ETG.Algolia.Providers
{    
    /// <summary>
    /// Class providing <see cref="SearchTaskAlgoliaInfo"/> management.
    /// </summary>
    public partial class SearchTaskAlgoliaInfoProvider : AbstractInfoProvider<SearchTaskAlgoliaInfo, SearchTaskAlgoliaInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="SearchTaskAlgoliaInfoProvider"/>.
        /// </summary>
        public SearchTaskAlgoliaInfoProvider()
            : base(SearchTaskAlgoliaInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="SearchTaskAlgoliaInfo"/> objects.
        /// </summary>
        public static ObjectQuery<SearchTaskAlgoliaInfo> GetSearchTaskAlgolias()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="SearchTaskAlgoliaInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="SearchTaskAlgoliaInfo"/> ID.</param>
        public static SearchTaskAlgoliaInfo GetSearchTaskAlgoliaInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="SearchTaskAlgoliaInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="SearchTaskAlgoliaInfo"/> to be set.</param>
        public static void SetSearchTaskAlgoliaInfo(SearchTaskAlgoliaInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="SearchTaskAlgoliaInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="SearchTaskAlgoliaInfo"/> to be deleted.</param>
        public static void DeleteSearchTaskAlgoliaInfo(SearchTaskAlgoliaInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="SearchTaskAlgoliaInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="SearchTaskAlgoliaInfo"/> ID.</param>
        public static void DeleteSearchTaskAlgoliaInfo(int id)
        {
            SearchTaskAlgoliaInfo infoObj = GetSearchTaskAlgoliaInfo(id);
            DeleteSearchTaskAlgoliaInfo(infoObj);
        }
    }
}
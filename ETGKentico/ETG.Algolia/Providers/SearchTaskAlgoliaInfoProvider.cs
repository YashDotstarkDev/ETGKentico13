using System;
using ETG.Algolia.Classes;
using CMS.Helpers;
using CMS.Search;

namespace ETG.Algolia.Providers
{
    public partial class SearchTaskAlgoliaInfoProvider
    {
        protected override void SetInfo(SearchTaskAlgoliaInfo infoObj)
        {
            if (infoObj != null && infoObj.SearchTaskAlgoliaCreated == DateTimeHelper.ZERO_TIME)
            {
                var searchTaskAlgoliaInfo = infoObj;
                searchTaskAlgoliaInfo.SearchTaskAlgoliaPriority = searchTaskAlgoliaInfo.SearchTaskAlgoliaType == SearchTaskTypeEnum.Rebuild ? 1 : 0;
                infoObj.SetValue("SearchTaskAlgoliaCreated", DateTime.Now);
            }

            base.SetInfo(infoObj);
        }
    }
}
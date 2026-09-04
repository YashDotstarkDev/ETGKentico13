using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Algolia.SearchDocumentCreators;

namespace ETG.Algolia.Factories
{
    public static class SearchDocumentCreatorFactory
    {
        public static BaseSearchDocumentCreator GetDocumentCreator(string indexCodeName, SearchDocument searchDocument, ISearchFields searchFields)
        {
            if (indexCodeName.ToLower().Contains("tour"))
            {
                return new TourSearchDocumentCreator(searchDocument, searchFields);
            }
            if (indexCodeName.ToLower().Contains("article"))
            {
                return new ArticleSearchDocumentCreator(searchDocument, searchFields);
            }
            if (indexCodeName.ToLower().Contains("content"))
            {
                return new ContentSearchDocumentCreator(searchDocument, searchFields);
            }
            
            return new CombinedSearchDocumentCreator(searchDocument, searchFields);
        }
    }
}

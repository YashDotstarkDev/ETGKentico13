using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Search
{
    public class SearchConfiguration : ISearchConfiguration
    {
        public string ApplicationId => ConfigurationManager.AppSettings["SearchApplicationID"];

        public string APIKey => ConfigurationManager.AppSettings["SearchAPIKey"];

        public string IndexTour => ConfigurationManager.AppSettings["SearchIndexTour"];

        public string IndexArticle => ConfigurationManager.AppSettings["SearchIndexArticle"];

        public string IndexContents => ConfigurationManager.AppSettings["SearchIndexContents"];
        public string IndexTypeAhead => ConfigurationManager.AppSettings["SearchIndexTypeAhead"];
    }
}

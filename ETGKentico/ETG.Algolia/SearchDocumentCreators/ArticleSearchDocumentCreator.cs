using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CMS.DataEngine;
using CMS.Helpers;
using CommonServiceLocator;
using ETG.Data.Factories;
using ETG.Data.Repositories.Modules;
using Newtonsoft.Json.Linq;

namespace ETG.Algolia.SearchDocumentCreators
{
    public class ArticleSearchDocumentCreator : BaseSearchDocumentCreator
    {
        public ArticleSearchDocumentCreator(SearchDocument searchDocument, ISearchFields searchFields) : base(searchDocument, searchFields)
        {
            CamelCase = true;
            FieldExcludeList.AddRange(
                new []
                {
                    "NodeID",
                    "classname",
                    "NodeAlias",
                    "DocumentPageDescription",
                    "DocumentIsArchived",
                    "DocumentPageDescription",
                    "DocumentPublishTo",
                    "DocumentPublishFrom",
                    "DocumentIsArchived",
                    "DocumentName",
                    "DocumentPageKeyWords",
                    "ArticleExperiences",
                    "ArticleCategories",
                    "ArticleDestinations",
                });
            //FieldProcessors.Add(new Tuple<string, Func<object, object>>("ArticleCategories", ArticleCategoryProcessor));
            FieldProcessors.Add(new Tuple<string, Func<object, object>>("ArticleHeroImage", ImgixifyProcessor));

        }

        protected override void UrlProcessor(JObject doc)
        {
            var nodeAlias = ValidationHelper.GetString(searchDocument.GetValue("NodeAlias"), string.Empty);
            if (string.IsNullOrWhiteSpace(nodeAlias))
            {
                return;
            }

            doc.Add("url", JToken.FromObject($"/articles/{nodeAlias.ToLower()}"));
        }


        protected override void AddAdditionalFields(JObject doc)
        {
            ProcessorHelper.DestinationProcessor( searchDocument,"ArticleDestinations","destinations", doc);
            ProcessorHelper.ExperienceProcessor(searchDocument, "ArticleExperiences", "experiences", doc);
            ArticleCategoryProcessor(doc);
        }

        private void ArticleCategoryProcessor(JObject doc)
        {

            var articleCategoryRepository = ServiceLocator.Current.GetInstance<IArticleCategoryRepository>(); // ServiceFactory.GetArticleCategoryRepositoryObject();
            var articleCategories = ValidationHelper.GetString(searchDocument.GetValue("ArticleCategories"), string.Empty);
            var categories = articleCategoryRepository.GetArticleCategories(articleCategories);


            if (categories.IsNullOrEmpty())
            {
                doc.Add("categories", JToken.FromObject(new List<string>()));
                return;
            }
            
           
            doc.Add("categories", JToken.FromObject(categories.Select(a => a.Name).ToList()));


        }

    }
}

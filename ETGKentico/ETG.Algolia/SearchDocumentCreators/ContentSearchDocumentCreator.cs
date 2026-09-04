using System;
using CMS.DataEngine;
using CMS.Helpers;
using Newtonsoft.Json.Linq;

namespace ETG.Algolia.SearchDocumentCreators
{
    public class ContentSearchDocumentCreator : BaseSearchDocumentCreator
    {
        public ContentSearchDocumentCreator(SearchDocument searchDocument, ISearchFields searchFields) : base(searchDocument, searchFields)
        {
            FieldProcessors.Add(new Tuple<string, Func<object, object>>("SearchImage", ImgixifyProcessor));
        }

        protected override void UrlProcessor(JObject doc)
        {
            var nodeAliasPath = ValidationHelper.GetString(searchDocument.GetValue("NodeAliasPath"), string.Empty).ToLower();

            if (string.IsNullOrWhiteSpace(nodeAliasPath))
            {
                return;
            }

            if (nodeAliasPath.StartsWith("/career-roles/"))
            {
                var nodeAlias = ValidationHelper.GetString(searchDocument.GetValue("NodeAlias"), string.Empty);
                if (string.IsNullOrWhiteSpace(nodeAlias))
                {
                    return;
                }

                doc.Add("Url", JToken.FromObject($"/careers/{nodeAlias.ToLower()}"));
                return;
            }

            doc.Add("Url", JToken.FromObject(nodeAliasPath.ToLower()));
        }
    }
}
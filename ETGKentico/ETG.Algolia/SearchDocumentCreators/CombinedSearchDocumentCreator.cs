using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CMS.DataEngine;
using CMS.Helpers;
using CommonServiceLocator;
using Devotion.Web.Base.Extensions;
using ETG.Data.Destination.Services;
using ETG.Data.Factories;
using Newtonsoft.Json.Linq;

namespace ETG.Algolia.SearchDocumentCreators
{
    public class CombinedSearchDocumentCreator : BaseSearchDocumentCreator
    {
        public CombinedSearchDocumentCreator(SearchDocument searchDocument, ISearchFields searchFields) : base(searchDocument, searchFields)
        {
            FieldProcessors.Add(new Tuple<string, Func<object, object>>("SearchImage", ImgixifyProcessor));
        }
        protected override void UrlProcessor(JObject doc)
        {
            var nodeAliasPath = ValidationHelper.GetString(searchDocument.GetValue("NodeAliasPath"), string.Empty);
            if (string.IsNullOrWhiteSpace(nodeAliasPath))
            {
                return;
            }
            if (nodeAliasPath.ToLower().Contains("/tour-folder/"))
            {
                var nodeAlias = ValidationHelper.GetString(searchDocument.GetValue("NodeAlias"), string.Empty);
                if (string.IsNullOrWhiteSpace(nodeAlias))
                {
                    return;
                }
                var countryGuid = ValidationHelper.GetGuid(searchDocument.GetValue("TourPrimaryCountry"), Guid.Empty);

                if (countryGuid == Guid.Empty)
                {
                    return;
                }

                var destinationService = ServiceLocator.Current.GetInstance<IDestinationService>(); //ServiceFactory.GetDestinationServiceObject();
                var destination = destinationService.GetDestination(countryGuid);
                doc.Add("Url", JToken.FromObject($"/{destination?.Name?.Replace(" ", "-")}/{nodeAlias.ToLower()}"));
            }else if (nodeAliasPath.ToLower().Contains("/articles-folder/"))
            {
                var nodeAlias = ValidationHelper.GetString(searchDocument.GetValue("NodeAlias"), string.Empty);
                doc.Add("Url", JToken.FromObject($"/articles/{nodeAlias.ToLower()}"));
            }
            else
            {
                doc.Add("Url", JToken.FromObject(nodeAliasPath.ToLower()));
            }
            
        }

    }
}

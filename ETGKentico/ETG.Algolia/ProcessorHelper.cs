using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using CMS.DataEngine;
using CMS.Helpers;
using CommonServiceLocator;
using Devotion.Web.Base.Extensions;
using ETG.Data.Destination.Services;
using ETG.Data.Experience.Services;
using Newtonsoft.Json.Linq;

namespace ETG.Algolia
{
    public static class ProcessorHelper
    {
        

        public static void DestinationProcessor(SearchDocument searchDocument, string sourceField, string newField, JObject doc)
        {

            var destinationService = ServiceLocator.Current.GetInstance<IDestinationService>(); //ServiceFactory.GetDestinationServiceObject();
            var destinationGuids = ValidationHelper.GetString(searchDocument.GetValue(sourceField), string.Empty);
            var guidList = destinationGuids.ToGuidList(';');
            var destinations = destinationService.GetDestinations(guidList);

            if (destinations.IsNullOrEmpty())
            {
                doc.Add(newField, JToken.FromObject(new List<string>()));
                return;
            }

            doc.Add(newField, JToken.FromObject(destinations.Select(a=>a.Name).ToList()));
        }

        public static void ExperienceProcessor(SearchDocument searchDocument, string sourceField, string newField, JObject doc)
        {
            var experienceService = ServiceLocator.Current.GetInstance<IExperienceService>();// ServiceFactory.GetExperienceServiceObject();
            var experienceGuids = ValidationHelper.GetString(searchDocument.GetValue(sourceField), string.Empty);
            var guidList = experienceGuids.ToGuidList(';');
            var experiences = experienceService.GetPreviewableExperienceSummaries(guidList);

            if (experiences.IsNullOrEmpty())
            {
                doc.Add(newField, JToken.FromObject(new List<string>()));
                return;
            }
            doc.Add(newField, JToken.FromObject(experiences.Select(a => a.Name).ToList()));
        }

        /*public static object DestinationProcessor(object arg)
        {
            var destinationService = ServiceFactory.GetDestinationServiceObject();

            var destinationGuids = ValidationHelper.GetString(arg, string.Empty);
            var guidList = destinationGuids.ToGuidList(';');
            var destinations = destinationService.GetDestinations(guidList);

            if (destinations.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return destinations[0].Name;
        }

        public static object ExperienceProcessor(object arg)
        {
            var experienceService = ServiceFactory.GetExperienceServiceObject();
            var experienceGuids = ValidationHelper.GetString(arg, string.Empty);
            var guidList = experienceGuids.ToGuidList(';');
            var experiences = experienceService.GetExperienceSummaries(guidList);

            if (experiences.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return string.Join(", ", experiences.Select(a => a.Name)); ;
        }*/


    }
}

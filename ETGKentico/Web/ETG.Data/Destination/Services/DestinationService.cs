using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Devotion.Web.Base.Extensions;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Cache;
using ETG.Data.Destination.Models;
using ETG.Data.Destination.Repositories;
using ETG.Data.Factories;

namespace ETG.Data.Destination.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly IDestinationRepository _destinationRepository;
        private readonly ICacheService _cacheService;

        public DestinationService(ICacheService cacheService, IDestinationRepository destinationRepository)
        {
            _cacheService = cacheService;
            _destinationRepository = destinationRepository;
        }

        private List<DestinationSummaryModel> GetAllDestinationsInternal(bool includeUnpublished = false)
        {
            if (includeUnpublished)
            {
                return DestinationProvider.GetDestinations().OnCurrentSite().OrderBy("NodeOrder").Published(false).Select(
                    ModelFactory.CreateDestinationSummaryModel).ToList();
            }
            return DestinationProvider.GetDestinations().OnCurrentSite().OrderBy("NodeOrder").Select(
                ModelFactory.CreateDestinationSummaryModel).ToList();
        }
        

        public List<DestinationSummaryModel> GetAllDestinations(bool includeUnpublished = false)
        {
            return _cacheService.GetDocumentDependentOnAll(()=>GetAllDestinationsInternal(includeUnpublished),
                $"allDestinations{includeUnpublished}", Core.PageTypes.Destination.CLASS_NAME);
        }

        public DestinationModel GetDestinationFullPage(string nodeAliasPath)
        {
            return _cacheService.GetDocumentDependentOnPath(() => _destinationRepository.GetDestination(nodeAliasPath),
                "destination", nodeAliasPath);
        }

        public bool IsDestinationUnpublished(string nodeAliasPath)
        {
            return _cacheService.GetDocumentDependentOnPath(() => _destinationRepository.IsDestinationUnpublished(nodeAliasPath),
                "destinationunpublished", nodeAliasPath);
        }

        public List<DestinationSummaryModel> GetMainDestinations(bool orderByName = false, bool includeUnpublished = false)
        {
            return !orderByName
                ? GetAllDestinations(includeUnpublished).Where(a => a.IsMainDestination).ToList()
                : GetAllDestinations(includeUnpublished).Where(a => a.IsMainDestination).OrderBy(a => a.Name).ToList();
        }

        public List<DestinationSummaryModel> GetDestinationRegions(string mainDestinationPath)
        {
            return mainDestinationPath.IsNullOrEmpty()
                ? null
                : GetAllDestinations()
                    .Where(a => a.Path.ToLower().StartsWith(mainDestinationPath.EndWithSlash().ToLower()))
                    .OrderBy(a => a.Name).ToList();
        }

        public List<DestinationSummaryModel> GetDestinationRegions(List<Guid> mainDestinationGuids)
        {
            var regionList = new List<DestinationSummaryModel>();
            var mainDestinations = GetAllDestinations().Where(a => mainDestinationGuids.Contains(a.DestinationGuid))
                .ToList();

            foreach (var destination in mainDestinations)
            {
                regionList.AddRange(GetAllDestinations()
                    .Where(a => a.Path.ToLower().StartsWith(destination.Path.ToLower())));
            }

            return regionList.IsNullOrEmpty() ? mainDestinations : regionList;
        }

        public DestinationSummaryModel GetDestination(Guid guid)
        {
            return GetAllDestinations().FirstOrDefault(a => a.DestinationGuid == guid);
        }

        public List<DestinationSummaryModel> GetDestinations(List<Guid> guids)
        {
            return guids.IsNullOrEmpty()
                ? null
                : GetAllDestinations().Where(a => guids.Contains(a.DestinationGuid)).ToList();
        }

        public bool IsMainCountry(string destinationName, bool includeUnpublished = false)
        {
            return GetMainDestinations(false, includeUnpublished).Any(a => a.Name.Trim().ToLower().Equals(destinationName.Trim().ToLower()));
        }

        public DestinationSummaryModel GetDestination(string nodeAliasPath)
        {
            return GetAllDestinations().FirstOrDefault(a => a.Path.ToLower().Equals(nodeAliasPath.ToLower()));
        }

        public DestinationSummaryModel GetDestinationByNodeAlias(string nodeAlias)
        {
            return GetAllDestinations().FirstOrDefault(a => a.PageAlias.ToLower().Equals(nodeAlias.ToLower()));
        }

        public string GetDestinationEmailNotification(string destinationName)
        {
            if (destinationName.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return GetAllDestinations().Where(a => a.Name.ToLower().Equals(destinationName.ToLower()))
                .Select(a => a.EnquiryNotificationEmail).FirstOrDefault();
        }
    }
}
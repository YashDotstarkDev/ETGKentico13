using ETG.Data.Destination.Models;
using System;
using System.Collections.Generic;

namespace ETG.Data.Destination.Services
{
    public interface IDestinationService
    {
        DestinationModel GetDestinationFullPage(string nodeAliasPath);
        bool IsDestinationUnpublished(string nodeAliasPath);
        List<DestinationSummaryModel> GetMainDestinations(bool orderByName = false, bool includeUnpublished = false);
        List<DestinationSummaryModel> GetAllDestinations(bool includeUnpublished = false);
        DestinationSummaryModel GetDestination(Guid guid);
        DestinationSummaryModel GetDestinationByNodeAlias(string nodeAlias);
        DestinationSummaryModel GetDestination(string nodeAliasPath);
        List<DestinationSummaryModel> GetDestinations(List<Guid> guids);
        List<DestinationSummaryModel> GetDestinationRegions(List<Guid> mainDestinationGuids); 
        List<DestinationSummaryModel> GetDestinationRegions(string mainDestinationPath);
        bool IsMainCountry(string destinationName, bool includeUnpublished = false);
        string GetDestinationEmailNotification(string destinationName);
    }
}

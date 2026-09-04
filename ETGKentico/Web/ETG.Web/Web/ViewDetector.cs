using Castle.Core.Internal;
using ETG.Core.Constants;
using ETG.Data.Destination.Services;
using ETG.Data.Experience.Services;
using ETG.Data.Tour.Repositories;
using System;

namespace ETG.Web.Web
{
    public class ViewDetector : IViewDetector
    {
        private ITourProductRepository _tourRepository;
        private readonly ICruiseTypeRepository _cruiseTypeRepository;
        private readonly IDestinationService _destinationService;
        private readonly IExperienceService _experienceService;

        public ViewDetector(ITourProductRepository tourRepository, ICruiseTypeRepository cruiseTypeRepository, IDestinationService destinationService, IExperienceService experienceService)
        {
            _tourRepository = tourRepository;
            _cruiseTypeRepository = cruiseTypeRepository;
            _destinationService = destinationService;
            _experienceService = experienceService;
        }

        private bool CheckIfTourUrl(string url)
        {
            var arr = url.Split('/');
            if (arr.Length != 3)
            {
                return false;
            }

            var tour = _tourRepository.GetTourByPageAlias(arr[2]);
            if (tour?.TourSummaryInfo == null)
            {
                return false;
            }

            var primaryCountry = _destinationService.GetDestination(tour.TourSummaryInfo.PrimaryCountryGuid);
            var primaryExperience = _experienceService.GetExperienceSummary(tour.TourSummaryInfo.PrimaryExperienceGuid);
            var cruiseType = _cruiseTypeRepository.GetCruiseType(tour.TourSummaryInfo.CruiseType);

            if (primaryCountry != null && primaryCountry.Name.Trim().Equals(arr[1].Replace("-", " "), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if (primaryExperience != null && primaryExperience.Path.Split('/')[2].Equals(arr[1], StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if (!string.IsNullOrEmpty(cruiseType.Key) && tour.TourSummaryInfo.IsCruise && cruiseType.Key.Equals(arr[1], StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public string GetView(string url)
        {
            if (url.IsNullOrEmpty())
            {
                return null;
            }

            if (CheckIfTourUrl(url))
            {
                return ViewConstants.TOUR;
            }

            return null;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CMS.DocumentEngine;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Cache;
using ETG.Data.Destination.Repositories;
using ETG.Data.Models.PageTypes;
using ETG.Data.TravelType.Models;
using ETG.Data.TravelType.Repositories;

namespace ETG.Data.TravelType.Services
{
    public class TravelTypeService : ITravelTypeService
    {
        private readonly ITravelTypeRepository _travelTypeRepository;
        private readonly ICacheService _cacheService;

        public TravelTypeService(ICacheService cacheService, ITravelTypeRepository travelTypeRepository)
        {
            _cacheService = cacheService;
            _travelTypeRepository = travelTypeRepository;
        }

        public TravelTypeDetailModel GetTravelTypeDetail(string nodeAliasPath)
        {
            return _cacheService.GetDocumentDependentOnPath(
                () => _travelTypeRepository.GetTravelTypeDetail(nodeAliasPath),
                "travelTypeDetail", nodeAliasPath);
        }

        public IEnumerable<ThemedPackageModel> GetThemedPackages(string nodeAliasPath)
        {
            return  _cacheService.GetDocumentDependentOnPath(() =>
                    ThemedPackageProvider.GetThemedPackages().Path(nodeAliasPath, PathTypeEnum.Children).ToList()
                        .Select(x => new ThemedPackageModel()
                        {
                            Heading = x.ThemedPackageHeading,
                            Description = x.ThemedPackageDescription,
                            ImagePath = x.ThemedPackageImage,
                            CtaLabel = x.ThemedPackageCTALabel,
                            CtaUrl = x.ThemedPackageCTAUrl
                        }), "themedPackage",
                nodeAliasPath);

        }

        public TravelTypeLandingModel GetLanding(string nodeAliasPath)
        {
            return _cacheService.GetDocumentDependentOnPath(
                () => _travelTypeRepository.GetLanding(nodeAliasPath),
                "travelTypeDetail", nodeAliasPath);
        }

        public IEnumerable<TravelTypeDetailSummaryModel> GetTravelTypeDetails(string nodeAliasPath)
        {
            return _cacheService.GetDocumentDependentOnPath(
                () => _travelTypeRepository.GetTravelTypeDetails(nodeAliasPath),
                "travelTypeDetailChildren", nodeAliasPath);
        }
    }
}
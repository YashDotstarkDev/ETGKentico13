using Castle.Core.Internal;
using ETG.Core.Kentico;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Tour.Factories;
using ETG.Data.Tour.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.EventLog;
using ETG.Core.Constants;

namespace ETG.Data.Tour.Repositories
{
    public class TourProductRepository : ITourProductRepository
    {
        private readonly ITourModelFactory _tourModelFactory;

        public TourProductRepository(ISiteContext siteContext, ITourModelFactory tourModelFactory)
        {
            _tourModelFactory = tourModelFactory;
        }


        public TourModel GetTourByPageAlias(string alias)
        {
            return _tourModelFactory.CreateTourModel(
                TourProvider.GetPreviewableTours()
                    .TopN(1)
                    .OnCurrentSite()
                    .WhereLike(nameof(Core.PageTypes.Tour.NodeAlias), alias)
                    .FirstOrDefault());
        }

        public TourModel GetTourByTourCode(string tourCode)
        {
            return _tourModelFactory.CreateTourModel(
                TourProvider.GetTours()
                    .TopN(1)
                    .OnCurrentSite()
                    .WhereLike(nameof(Core.PageTypes.Tour.TourCode), tourCode)
                    .FirstOrDefault());
        }

        public TourModel GetPreviewableTourByTourCode(string tourCode)
        {
            return _tourModelFactory.CreateTourModel(
                TourProvider.GetPreviewableTours()
                    .TopN(1)
                    .OnCurrentSite()
                    .WhereLike(nameof(Core.PageTypes.Tour.TourCode), tourCode)
                    .FirstOrDefault());
        }

        public List<TourSummaryInfoModel> GetToursByTourCodes(List<string> tourCodes, bool includeHiddenUpgradeTours = false)
        {
            if (tourCodes.IsNullOrEmpty())
            {
                return null;
            }

            if (!includeHiddenUpgradeTours)
            {
                return TourProvider.GetTours()
                    .OnCurrentSite()
                    .WhereIn(nameof(Core.PageTypes.Tour.TourCode), tourCodes)
                    .And(new WhereCondition($"({nameof(Core.PageTypes.Tour.TourIsUpgrade)} IS NULL OR {nameof(Core.PageTypes.Tour.TourIsUpgrade)} <> {TourUpgrade.HiddenUpgradeTour})"))
                    .Select(_tourModelFactory.CreateSummaryInfoModel).ToList();
            }
            
            return TourProvider.GetTours()
                .OnCurrentSite()
                .WhereIn(nameof(Core.PageTypes.Tour.TourCode), tourCodes)
                .Select(_tourModelFactory.CreateSummaryInfoModel).ToList();
        }

        public List<TourSummaryInfoModel> GetToursByExperience(Guid experienceGuid, int topN)
        {
            return experienceGuid == Guid.Empty
                ? null
                : TourProvider.GetTours()
                    .TopN(topN)
                    .OnCurrentSite()
                    .And(new WhereCondition($"({nameof(Core.PageTypes.Tour.TourIsUpgrade)} IS NULL OR {nameof(Core.PageTypes.Tour.TourIsUpgrade)} <> {TourUpgrade.HiddenUpgradeTour})"))
                    .WhereLike(nameof(Core.PageTypes.Tour.TourExperiences), $"%{experienceGuid}%")
                    .Select(_tourModelFactory.CreateSummaryInfoModel).ToList();
        }

        public List<TourSummaryInfoModel> GetToursByDestination(Guid destinationGuid, int topN)
        {
            return destinationGuid == Guid.Empty
                ? null
                : TourProvider.GetTours()
                    .TopN(topN)
                    .OnCurrentSite()
                    .WhereEquals(nameof(Core.PageTypes.Tour.TourPrimaryCountry), destinationGuid)
                    .And(new WhereCondition($"({nameof(Core.PageTypes.Tour.TourIsUpgrade)} IS NULL OR {nameof(Core.PageTypes.Tour.TourIsUpgrade)} <> {TourUpgrade.HiddenUpgradeTour})"))
                    .Select(_tourModelFactory.CreateSummaryInfoModel).ToList();
        }

        public List<TourSummaryInfoModel> GetTours(int topN)
        {
            return TourProvider.GetTours()
                .TopN(topN)
                .OnCurrentSite()
                .And(new WhereCondition($"({nameof(Core.PageTypes.Tour.TourIsUpgrade)} IS NULL OR {nameof(Core.PageTypes.Tour.TourIsUpgrade)} <> {TourUpgrade.HiddenUpgradeTour})"))
                .OrderBy(x => Guid.NewGuid())
                .Select(_tourModelFactory.CreateSummaryInfoModel).ToList();
        }

        public List<TourSummaryInfoModel> GetToursByDestinations(List<Guid> destinationGuids, int topN)
        {
            if (destinationGuids.IsNullOrEmpty())
            {
                return null;
            }

            return TourProvider.GetTours().TopN(topN)
                .OnCurrentSite()
                .And(new WhereCondition($"({nameof(Core.PageTypes.Tour.TourIsUpgrade)} IS NULL OR {nameof(Core.PageTypes.Tour.TourIsUpgrade)} <> {TourUpgrade.HiddenUpgradeTour})"))
                .WhereIn(nameof(Core.PageTypes.Tour.TourPrimaryCountry), destinationGuids)
                .OrderBy(x => Guid.NewGuid()).Select(_tourModelFactory.CreateSummaryInfoModel).ToList();
        }


        public List<TourSummaryInfoModel> GetToursByTourType(string tourType, int topN)
        {
            if (tourType.IsNullOrEmpty())
            {
                return null;
            }

            return TourProvider.GetTours()
                .And(new WhereCondition($"({nameof(Core.PageTypes.Tour.TourIsUpgrade)} IS NULL OR {nameof(Core.PageTypes.Tour.TourIsUpgrade)} <> {TourUpgrade.HiddenUpgradeTour})"))
                .WhereLike("TourTypes", $"%{tourType}%").OnCurrentSite().AllCultures().TopN(topN).Select(_tourModelFactory.CreateSummaryInfoModel).ToList();

        }

        public List<TourSummaryInfoModel> GetToursByCruiseType(int cruiseType, int topN)
        {
            if (cruiseType == 0)
            {
                return null;
            }
            return TourProvider.GetTours()
                .And(new WhereCondition($"({nameof(Core.PageTypes.Tour.TourIsUpgrade)} IS NULL OR {nameof(Core.PageTypes.Tour.TourIsUpgrade)} <> {TourUpgrade.HiddenUpgradeTour})"))
                .WhereEquals("TourCruiseType", cruiseType).OnCurrentSite().TopN(topN).Select(_tourModelFactory.CreateSummaryInfoModel).ToList();

        }
        public List<TourBookingAdditions> GetTourBookingAdditions(List<string> tourCodes)
        {
            EventLogProvider.ProviderObject.Set(new EventLogInfo("I", "Debug", "query")
            {
                EventDescription = TourProvider.GetTours().AllCultures().WhereIn(nameof(Core.PageTypes.Tour.TourCode), tourCodes).ToString()
            });
            var tours = TourProvider.GetTours().AllCultures().WhereIn(nameof(Core.PageTypes.Tour.TourCode), tourCodes)
                .Select(t=>new TourBookingAdditions
                {
                    tourCode = t.TourCode,
                    tourNodeAliasPath = t.NodeAliasPath
                }).ToList();

            var allRoomUpgrades = RoomUpgradeProvider.GetRoomUpgrades().AllCultures().ToList();
            var allExtras = OptionalExtrasProvider.GetOptionalExtras().AllCultures().ToList();

            tours.ForEach(t=>t.RoomUpgrades = allRoomUpgrades.Where(r=>r.NodeAliasPath.StartsWith(t.tourNodeAliasPath + "/")).OrderBy(a=>a.RoomUpgradeNumber).ToList());
            tours.ForEach(t => t.OptionalExtras = allExtras.Where(r => r.NodeAliasPath.StartsWith(t.tourNodeAliasPath + "/")).OrderBy(a => a.OptionalExtrasNumber).ToList());

            return tours;
        }

        public List<Core.PageTypes.Tour> GetToursWithDiscounts(List<Guid> discountGuids)
        {
            return TourProvider.GetTours().OnCurrentSite()
                .WhereIn(nameof(Core.PageTypes.Tour.TourDiscountAndOffer), discountGuids)
                .ToList();
        }

        public List<string> GetInvalidTourCodes(List<string> allTourCodes)
        {
            var validList = TourProvider.GetTours().OnCurrentSite()
                .WhereIn(nameof(ETG.Core.PageTypes.Tour.TourCode), allTourCodes).Columns("TourCode")
                .Select(a => a.TourCode).ToList();

            return allTourCodes.Except(validList).Where(a=>!a.IsNullOrEmpty()).Where(a=>!a.Contains("XXXXX")).ToList();
        }
    }
}
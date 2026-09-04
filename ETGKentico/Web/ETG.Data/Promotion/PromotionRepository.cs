using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Castle.Core.Internal;
using CMS.Core;
using CMS.DataEngine;
using CMS.SiteProvider;
using Devotion.Cache;
using ETG.Core.PageTypes.Providers;
using ETG.Core.Promotion;
using ETG.Core.Utility;
using ETG.Data.Cache;
using ETG.Data.Tour.Models;

namespace ETG.Data.Promotion
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly IClock _clock;
        private readonly ICacheProvider _cacheProvider;
        private readonly IEventLogService _eventLogService;
        public PromotionRepository(IClock clock, ICacheProvider cacheProvider)
        {
            _clock = clock;
            _cacheProvider = cacheProvider;
            _eventLogService = Service.Resolve<IEventLogService>();
        }

        private WhereCondition GetDestinationAndTourCodeWhereCondition(TourModel tour)
        {
            if (tour?.TourSummaryInfo == null)
            {
                return new WhereCondition();
            }
            var destinationWhereCondition = new WhereCondition();
            destinationWhereCondition = destinationWhereCondition.WhereNull(nameof(PromotionInfo.PromotionDestinations))
                .Or().Where($"LTRIM(RTRIM({nameof(PromotionInfo.PromotionDestinations)})) = ''").Or().WhereLike(nameof(PromotionInfo.PromotionDestinations), $"%{tour.TourSummaryInfo.PrimaryCountryGuid}%");
            var tourCodeWhereCondition = new WhereCondition();
            tourCodeWhereCondition = tourCodeWhereCondition.WhereNull(nameof(PromotionInfo.PromotionPackageCodes))
                .Or().Where($"LTRIM(RTRIM({nameof(PromotionInfo.PromotionPackageCodes)})) = ''").Or().Where(
                    $"CHAR(13)+CHAR(10) + {nameof(PromotionInfo.PromotionPackageCodes)} + CHAR(13)+CHAR(10) LIKE '%' + CHAR(13)+CHAR(10) + '{tour.TourSummaryInfo.TourCode}' + CHAR(13)+CHAR(10) + '%'");

            return new WhereCondition().Where(destinationWhereCondition).Where(tourCodeWhereCondition);
        }

        public PromotionInfo GetPromotionInfoForNonAgent(TourModel tour, DateTime date)
        {
            var tourWhereCondition = GetDestinationAndTourCodeWhereCondition(tour);
            return PromotionInfoProvider.GetPromotions()
                .WhereEmpty(nameof(PromotionInfo.PromotionPromoCode))
                .WhereLessOrEquals(nameof(PromotionInfo.PromotionFromDate), date)
                .WhereGreaterOrEquals(nameof(PromotionInfo.PromotionToDate), date)
                .Where(new WhereCondition().WhereNull(nameof(PromotionInfo.PromotionEmailDomains)).Or(new WhereCondition($"LTRIM(RTRIM({nameof(PromotionInfo.PromotionEmailDomains)})) = ''")))
                .Where(tourWhereCondition)
                .FirstOrDefault();
        }

        public PromotionInfo GetPromotionInfoForNonAgentForEDM(TourModel tour, DateTime date)
        {
            
            return _cacheProvider.GetCached(()=> GetPromotionInfoForNonAgentForEDMInternal(tour,date),
                $"promotionTourEDM-{tour.TourSummaryInfo.TourCode}-{date}", 
                $"{new GenericDependencyBuilder<PromotionInfo>(SiteContext.CurrentSiteName)}"
                );
        }
        private PromotionInfo GetPromotionInfoForNonAgentForEDMInternal(TourModel tour, DateTime date)
        {
            var tourWhereCondition = GetDestinationAndTourCodeWhereCondition(tour);
            var promotion = PromotionInfoProvider.GetPromotions()
                .WhereLessOrEquals(nameof(PromotionInfo.PromotionFromDate), date)
                .WhereGreaterOrEquals(nameof(PromotionInfo.PromotionToDate), date)
                .Where(new WhereCondition().WhereNull(nameof(PromotionInfo.PromotionEmailDomains)).Or(new WhereCondition($"LTRIM(RTRIM({nameof(PromotionInfo.PromotionEmailDomains)})) = ''")))
                .Where(tourWhereCondition)
                .FirstOrDefault();

            return promotion ?? new PromotionInfo();
        }


        public PromotionInfo GetPromotionInfoForAgent(TourModel tour, string email, DateTime date)
        {
            if (tour == null)
            {
                return null;
            }
            var tourWhereCondition = GetDestinationAndTourCodeWhereCondition(tour);
            var domain = "";
            
            if (!email.IsNullOrEmpty())
            {
               var index = email.IndexOf("@");
               if (index > -1)
               {
                   domain = email.Substring(index + 1);
               }
            }
           
            if (!domain.IsNullOrEmpty())
            {
                return PromotionInfoProvider.GetPromotions()
                .WhereEmpty(nameof(PromotionInfo.PromotionPromoCode))
                .WhereLessOrEquals(nameof(PromotionInfo.PromotionFromDate), date)
                .WhereGreaterOrEquals(nameof(PromotionInfo.PromotionToDate), date)
                .Where(tourWhereCondition)
                .Where(new WhereCondition($"CHAR(13)+CHAR(10) + {nameof(PromotionInfo.PromotionEmailDomains)} + CHAR(13)+CHAR(10) LIKE '%' + CHAR(13)+CHAR(10) + '{domain}' + CHAR(13)+CHAR(10) + '%'"))
                .FirstOrDefault();
            }

            return null;

        }

        public List<PromotionInfo> GetExpiredPromotions(int lastNDays)
        {
            return PromotionInfoProvider.GetPromotions()
                .WhereLessOrEquals(nameof(PromotionInfo.PromotionToDate), _clock.Today)
                .WhereGreaterOrEquals(nameof(PromotionInfo.PromotionToDate), _clock.Today.AddDays(-lastNDays))
                .ToList();
        }

        public List<Core.PageTypes.Tour> GetToursWithPromotions(List<PromotionInfo> promotions)
        {
            if (promotions.IsNullOrEmpty())
            {
                return null;
            }

            var tourCodesTexts = promotions.Where(a => !a.PromotionPackageCodes.IsNullOrEmpty())
                .Select(a => a.PromotionPackageCodes.Split('\n').Distinct()).ToList();

            var tourCodes = new List<string>();
            foreach (var text in tourCodesTexts)
            {
                tourCodes.AddRange(text);
            }

            tourCodes = tourCodes.Distinct().ToList();
            
            var destinationTexts = promotions.Where(a => !a.PromotionDestinations.IsNullOrEmpty())
                .Select(a => a.PromotionDestinations.Split('\n').Distinct()).ToList();

            var destinationsGuids = new List<string>();
            foreach (var text in destinationTexts)
            {
                destinationsGuids.AddRange(text);
            }

            var tours =TourProvider.GetTours().OnCurrentSite();

            if (!destinationsGuids.IsNullOrEmpty())
            {
                tours = tours.WhereIn(nameof(Core.PageTypes.Tour.TourPrimaryCountry), destinationsGuids);
            }

            if (!tourCodes.IsNullOrEmpty())
            {
                if (!destinationsGuids.IsNullOrEmpty())
                {
                    tours = tours.Or();
                }
                tours = tours.WhereIn(nameof(Core.PageTypes.Tour.TourCode), tourCodes);
            }

            return tours.ToList();
        }

        public List<PromotionInfo> GetAllPromotions(DateTime date)
        {
            return PromotionInfoProvider.GetPromotions()
                .WhereEmpty(nameof(PromotionInfo.PromotionPromoCode))
                .WhereLessOrEquals(nameof(PromotionInfo.PromotionFromDate), date)
                .WhereGreaterOrEquals(nameof(PromotionInfo.PromotionToDate), date).ToList();
        }
        
        public PromotionInfo GetPromotionInfoByPromoCode(string promoCode, TourModel tour, DateTime bookingDate, DateTime depatureDate)
        {
            if (string.IsNullOrWhiteSpace(promoCode))
            {
                return null;
            }
            
            var tourWhereCondition = GetDestinationAndTourCodeWhereCondition(tour);
            var promotions = PromotionInfoProvider.GetPromotions()
                .WhereEquals(nameof(PromotionInfo.PromotionPromoCode), promoCode)
                .WhereLessOrEquals(nameof(PromotionInfo.PromotionFromDate), bookingDate)
                .WhereGreaterOrEquals(nameof(PromotionInfo.PromotionToDate), bookingDate)
                .Where(new WhereCondition().WhereNull(nameof(PromotionInfo.PromotionEmailDomains))
                    .Or(new WhereCondition($"LTRIM(RTRIM({nameof(PromotionInfo.PromotionEmailDomains)})) = ''")))
                .Where(tourWhereCondition)
                .ToList();

            if (!promotions.Any())
            {
                return null;
            }

            //check if there's a matching promotion without applicable travel date requirement
            var promotion = promotions
                .FirstOrDefault(a => a.PromotionPackageDates == null || string.IsNullOrWhiteSpace(a.PromotionPackageDates));

            if (promotion != null)
            {
                return promotion;
            }

            foreach (var promoToCheck in promotions)
            {
                var appliedDateRange = GetPromotionDateRanges(promoToCheck.PromotionPackageDates);

                var inclusive = DepartureDateIsInclusive(appliedDateRange, depatureDate);

                if (inclusive)
                {
                    return promoToCheck;
                }
            }

            return null;

        }

        private bool DepartureDateIsInclusive(List<DateRange> appliedDateRanges, DateTime depatureDate)
        {
            if (appliedDateRanges.IsNullOrEmpty())
            {
                return false;
            }

            return appliedDateRanges.Any(a => a.FromDate <= depatureDate && depatureDate <= a.ToDate);
        }

        private List<DateRange> GetPromotionDateRanges(string promotionPackageDates)
        {
            if (string.IsNullOrWhiteSpace(promotionPackageDates))
            {
                return new List<DateRange>();
            }

            var stringDates = promotionPackageDates.Split('\n');

            var dateRanges = new List<DateRange>();
            foreach (var stringDate in stringDates)
            {
                if (stringDate.IsNullOrEmpty())
                {
                    continue;
                }

                DateRange dateRange = null;
                if (stringDate.Contains("-"))
                {
                    var dates = stringDate.Trim().Split('-');

                    if (dates.Length == 2)
                    {
                        var fromDate = EnsureDate(dates[0]);
                        var toDate = EnsureDate(dates[1]);

                        if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                        {
                            dateRange = new DateRange
                            {
                                FromDate = fromDate,
                                ToDate = toDate
                            };
                        }
                    }
                }
                else
                {
                    var date = EnsureDate(stringDate.Trim());

                    if (date != DateTime.MinValue)
                    {
                        dateRange = new DateRange
                        {
                            FromDate = date,
                            ToDate = date
                        };
                    }
                }

                if (dateRange != null)
                {
                    dateRanges.Add(dateRange);
                }
            }

            return dateRanges;
        }

        private DateTime EnsureDate(string stringDate)
        {
            if (stringDate.IsNullOrEmpty())
            {
                return DateTime.MinValue;
            }

            var arr = stringDate.Split('/');

            if (arr.Length != 3 && arr[2].Length != 4)
            {
                return DateTime.MinValue;
            }

            arr[0] = arr[0].PadLeft(2, '0');
            arr[1] = arr[1].PadLeft(2, '0');

            var newDateFormat = $"{arr[0]}/{arr[1]}/{arr[2]}".Trim();


            DateTime.TryParseExact(newDateFormat, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var newDate);

            return newDate;


        }
    }
}
using CMS.Base;
using CMS.DataEngine;
using Devotion.Cache;
using ETG.Booking.Pricing.Classes.Info;
using ETG.Booking.Pricing.Classes.Providers;
using ETG.Booking.Pricing.Enums;
using ETG.Booking.Pricing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;

namespace ETG.Booking.Pricing.Repositories
{
    public class BookingPriceRepository : IBookingPriceRepository
    {
        private readonly ICacheProvider _cacheProvider;

        public BookingPriceRepository(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        private List<DateBookingPrice> GetDepartureBookingPricesInternal(string tourCode, DateTime? from, DateTime? to,
            bool includePast = false)
        {
            var query = BookingPriceInfoProvider.GetBookingPrices()
                .WhereEquals(nameof(BookingPriceInfo.BookingPriceTourCode), tourCode);

            var dateWhere = new WhereCondition();
            bool hasFrom = false;
            if (from.GetValueOrDefault() > DateTime.MinValue && to.GetValueOrDefault() > DateTime.MinValue)
            {
                dateWhere = dateWhere.WhereLessOrEquals(nameof(BookingPriceInfo.BookingPriceStartDate), from)
                    .WhereGreaterOrEquals(nameof(BookingPriceInfo.BookingPriceEndDate), from);
                dateWhere = dateWhere.Or();
                dateWhere = dateWhere.Where(new WhereCondition()
                    .WhereLessOrEquals(nameof(BookingPriceInfo.BookingPriceStartDate), to)
                    .WhereGreaterOrEquals(nameof(BookingPriceInfo.BookingPriceEndDate), to));
                dateWhere = dateWhere.Or();
                // from < start < to
                dateWhere = dateWhere.Where(new WhereCondition()
                    .WhereGreaterOrEquals(nameof(BookingPriceInfo.BookingPriceStartDate), from)
                    .WhereLessOrEquals(nameof(BookingPriceInfo.BookingPriceStartDate), to));
                dateWhere = dateWhere.Or();
                // from < end < to
                dateWhere = dateWhere.Where(new WhereCondition()
                    .WhereGreaterOrEquals(nameof(BookingPriceInfo.BookingPriceEndDate), from)
                    .WhereLessOrEquals(nameof(BookingPriceInfo.BookingPriceEndDate), to));
            }
            else if (from.GetValueOrDefault() == DateTime.MinValue && to.GetValueOrDefault() > DateTime.MinValue)
            {
                dateWhere = dateWhere.Where(new WhereCondition()
                    .WhereLessOrEquals(nameof(BookingPriceInfo.BookingPriceEndDate), to).Or(new WhereCondition()
                        .WhereLessOrEquals(nameof(BookingPriceInfo.BookingPriceStartDate), to)
                        .WhereGreaterOrEquals(nameof(BookingPriceInfo.BookingPriceEndDate), to)));
            }
            else if (to.GetValueOrDefault() == DateTime.MinValue && from.GetValueOrDefault() > DateTime.MinValue)
            {
                dateWhere = dateWhere.WhereLessOrEquals(nameof(BookingPriceInfo.BookingPriceStartDate), from).Or()
                    .WhereGreaterOrEquals(nameof(BookingPriceInfo.BookingPriceEndDate), from);
            }

            if ((from.HasValue && from.Value > DateTime.MinValue) || (to.HasValue && to.Value > DateTime.MinValue))
            {
                query = query.Where(dateWhere);
            }

            query = query.WhereEqualsOrNull(nameof(BookingPriceInfo.IsEnabled), true);
            return query.OrderBy(nameof(BookingPriceInfo.BookingPriceStartDate)).Select(
                price => new DateBookingPrice
                {
                    StartDate = price.BookingPriceStartDate,
                    EndDate = price.BookingPriceEndDate,
                    TwinSharePrice = price.BookingPriceTwinSharePrice,
                    SingleSupplementalCost = price.BookingPriceSingleSupplementalCost,
                    TwinSharePreNightPrice = price.BookingPricePreNightTwinPrice,
                    TwinSharePostNightPrice = price.BookingPricePostNightTwinPrice,
                    SingleSupplementalPreNightCost = price.BookingPricePreNightSinglePrice,
                    SingleSupplementalPostNightCost = price.BookingPricePostNightSinglePrice
                }).ToList();
        }

        public List<DateBookingPrice> GetDepartureBookingPrices(string tourCode, DateTime? from, DateTime? to,
            bool includePast = false)
        {
            return _cacheProvider.GetCached(() => GetDepartureBookingPricesInternal(tourCode, from, to, includePast),
                $"GetDepartureBookingPrices{tourCode}{@from:ddMMyyyy}{@to:ddMMyyyy}{includePast}",
                $"{new ObjectDependencyBuilder<BookingRoomOptionInfo>().DependsOnAll()}{new ObjectDependencyBuilder<BookingPriceInfo>().DependsOnAll()}"
            );
        }

        private List<BookingOption> GetBookingOptions(List<BookingRoomOptionInfo> allOptions,
            RoomOptionTypeEnum optionType)
        {
            return allOptions.Where(o => o.BookingRoomOptionType == (int)optionType)
                .OrderBy(o => o.BookingRoomOptionOrder).Select(
                    o => new BookingOption
                    {
                        OptionLabel = o.BookingRoomOptionLabel,
                        PricePerPerson = o.BookingRoomOptionPricePerPerson,
                        SoloOptionLabel = o.BookingRoomOptionLabel2,
                        SoloPricePerPerson = o.BookingRoomOptionPricePerPerson2,
                        OptionGuid = o.BookingRoomOptionGuid,
                        OptionType = optionType
                    }).ToList();
        }

        private DepartureDateBookingOptions GetBookingOptionsInternal(string tourCode, DateTime departureDate)
        {
            var options = BookingRoomOptionInfoProvider.GetBookingRoomOptions().Source(option =>
                    option.Join<BookingPriceInfo>(nameof(BookingRoomOptionInfo.BookingRoomOptionBookingPriceID),
                        nameof(BookingPriceInfo.BookingPriceID)))
                .WhereEquals(nameof(BookingPriceInfo.BookingPriceTourCode), tourCode)
                .WhereLessOrEquals(nameof(BookingPriceInfo.BookingPriceStartDate), departureDate)
                .WhereGreaterOrEquals(nameof(BookingPriceInfo.BookingPriceEndDate), departureDate)
                .WhereGreaterThan(nameof(BookingRoomOptionInfo.BookingRoomOptionPricePerPerson), 0).ToList();


            return new DepartureDateBookingOptions
            {
                TwinShareRoomOptions = GetBookingOptions(options, RoomOptionTypeEnum.TwinShareRoomOption),
                SingleRoomOptions = GetBookingOptions(options, RoomOptionTypeEnum.SingleRoomOption),
                ExtraOptions = GetBookingOptions(options, RoomOptionTypeEnum.Extras)
            };
        }

        public DepartureDateBookingOptions GetBookingOptions(string tourCode, DateTime departureDate)
        {
            return _cacheProvider.GetCached(() => GetBookingOptionsInternal(tourCode, departureDate),
                $"GetBookingOptions{tourCode}{departureDate:ddMMyyyy}",
                new ObjectDependencyBuilder<BookingRoomOptionInfo>().DependsOnAll());
        }

        private DateBookingPrice GetDepartureBookingPriceInternal(string tourCode, DateTime departureDate)
        {
            return BookingPriceInfoProvider.GetBookingPrices()
                .WhereEquals(nameof(BookingPriceInfo.BookingPriceTourCode), tourCode)
                .WhereLessOrEquals(nameof(BookingPriceInfo.BookingPriceStartDate), departureDate)
                .WhereGreaterOrEquals(nameof(BookingPriceInfo.BookingPriceEndDate), departureDate).Select(
                    price => new DateBookingPrice
                    {
                        StartDate = price.BookingPriceStartDate,
                        EndDate = price.BookingPriceEndDate,
                        TwinSharePrice = price.BookingPriceTwinSharePrice,
                        SingleSupplementalCost = price.BookingPriceSingleSupplementalCost,
                        DaysToBalanceDueDate = price.BalanceDueDays,
                        DaysToSecondInstalment = price.SecondInstalmentDays,
                        SecondInstalmentPercentage = price.SecondInstalmentPercent,
                        TwinSharePreNightPrice = price.BookingPricePreNightTwinPrice,
                        TwinSharePostNightPrice = price.BookingPricePostNightTwinPrice,
                        SingleSupplementalPreNightCost = price.BookingPricePreNightSinglePrice,
                        SingleSupplementalPostNightCost = price.BookingPricePostNightSinglePrice
                    }).FirstOrDefault();
        }


        public DateBookingPrice GetDepartureBookingPrice(string tourCode, DateTime departureDate)
        {
            return _cacheProvider.GetCached(() => GetDepartureBookingPriceInternal(tourCode, departureDate),
                $"GetDepartureBookingPrice{tourCode}{departureDate:ddMMyyyy}",
                new ObjectDependencyBuilder<BookingPriceInfo>().DependsOnAll());
        }

        private double GetPreNightPrice(RoomOptionTypeEnum roomType, BookingRoomOptionInfo roomOption)
        {
            if (roomType != RoomOptionTypeEnum.TwinShareRoomOption)
            {
                return 0;
            }

            return roomOption.BookingRoomOptionTwinPreNightPrice;
        }
            
        private double GetPostNightPrice(RoomOptionTypeEnum roomType, BookingRoomOptionInfo roomOption)
        {
            //Only Twin/double room upgrades have pre/post nights price
            if (roomType != RoomOptionTypeEnum.TwinShareRoomOption)
            {
                return 0;
            }

            return roomOption.BookingRoomOptionTwinPostNightPrice;
        }
        
        public List<BookingOption> GetBookingOptions(List<Guid> roomOptionsGuids)
        {

            return BookingRoomOptionInfoProvider.GetBookingRoomOptions()
                .WhereIn(nameof(BookingRoomOptionInfo.BookingRoomOptionGuid), roomOptionsGuids)
                .Select(o => new BookingOption
                {
                    OptionGuid = o.BookingRoomOptionGuid,
                    OptionLabel = o.BookingRoomOptionLabel,
                    PricePerPerson = o.BookingRoomOptionPricePerPerson,
                    PreNightPricePerPerson = GetPreNightPrice((RoomOptionTypeEnum)o.BookingRoomOptionType, o),
                    PostNightPricePerPerson = GetPostNightPrice((RoomOptionTypeEnum)o.BookingRoomOptionType, o),
                    SoloOptionLabel = o.BookingRoomOptionLabel2,
                    SoloPricePerPerson = o.BookingRoomOptionPricePerPerson2,
                    OptionType = (RoomOptionTypeEnum)o.BookingRoomOptionType
                }).ToList();
        }

        public int GetLowestPriceFromBookNowPricing(string tourCode)
        {
            if (string.IsNullOrEmpty(tourCode))
            {
                return 0;
            }

            var where = new WhereCondition();
            where = where
                .WhereGreaterThan(nameof(BookingPriceInfo.BookingPriceEndDate), DateTime.Today);
            var price = BookingPriceInfoProvider.GetBookingPrices()
                .WhereEquals(nameof(BookingPriceInfo.BookingPriceTourCode), tourCode)
                .Where(where)
                .Select(a => a.BookingPriceTwinSharePrice); //.Min();

            if (price.Any())
            {
                return price.Min().ToInteger(0);
            }

            return 0;
        }

        public List<KeyValuePair<string, int>> GetLowestPricesFromBookNowPricing(List<string> tourCodes)
        {
            var where = new WhereCondition();
            where = where.WhereGreaterThan(nameof(BookingPriceInfo.BookingPriceEndDate), DateTime.Today);

            var ds = BookingPriceInfoProvider.GetBookingPrices()
                .Columns(nameof(BookingPriceInfo.BookingPriceTourCode))
                .AddColumn(new AggregatedColumn(AggregationType.Min,
                    nameof(BookingPriceInfo.BookingPriceTwinSharePrice)).As("Price"))
                .WhereIn(nameof(BookingPriceInfo.BookingPriceTourCode), tourCodes)
                .Where(where)
                .GroupBy(nameof(BookingPriceInfo.BookingPriceTourCode))
                .Result;

            var prices = new List<KeyValuePair<string, int>>();

            if (!DataHelper.DataSourceIsEmpty(ds))
            {
                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    prices.Add(new KeyValuePair<string, int>(
                        ds.Tables[0].Rows[i][nameof(BookingPriceInfo.BookingPriceTourCode)].ToString(),
                        ds.Tables[0].Rows[i]["Price"].ToInteger(0)));
                }
            }

            return prices;
        }
    }
}
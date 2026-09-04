using System.Collections.Generic;
using Castle.Core.Internal;
using ETG.Core.Constants;
using ETG.Core.PageTypes;
using ETG.Data.Destination.Models;
using ETG.Data.DestinationExpertTeam.Models;
using ETG.Data.Experience.Models;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Tour.Data;
using ETG.Data.Tour.Models;
using ETG.Data.TravelType.Models;

namespace ETG.Data.Factories
{
    public static class ModelFactory
    {
        public static DestinationSummaryModel CreateDestinationSummaryModel(ETG.Core.PageTypes.Destination a)
        {
            return new DestinationSummaryModel
            {
                Name = a.DestinationName,
                Path = a.NodeAliasPath,
                PageAlias = a.NodeAlias,
                Heading = a.DestinationHeading,
                HeroImage = a.DestinationHeroImage,
                HeroForegroundImage = a.DestinationHeroForegroundImage,
                DisableOverlay = a.DestinationHeroDisableOverlay,
                HeroAltText = a.DestinationHeroImageAltTag,
                CampaignTitle = a.DestinationCampaignTitle,
                Summary = a.DestinationSummary,
                DestinationGuid = a.NodeGUID,
                Latitude = a.DestinationLatitude,
                Longitude = a.DestinationLongitude,
                IsMainDestination = a.NodeLevel == 2,
                EthnicGroup = a.DestinationEthnicGroup,
                Culture = a.DestinationCulture,
                Geography = a.DestinationGeography,
                Currency = a.DestinationCurrency,
                OfficialLanguages = a.DestinationOfficialLanguages,
                EnquiryNotificationEmail = a.DestinationEnquiryNotificationEmail,
                IDInMap = a.DestinationMapID
            };
        }

        public static DestinationExpertTeamSummaryModel CreateDestinationExpertTeamSummaryModel(
            ETG.Core.PageTypes.DestinationExpertTeam a)
        {
            return new DestinationExpertTeamSummaryModel
            {
                DestinationExpertTeamGuid = a.NodeGUID,
                Name = a.DestinationExpertTeamName,
                HeroImage = a.DestinationExpertTeamHeroImage,
                HeroAltText = a.DestinationExpertTeamHeroImageAltTag,
                SummaryText = a.DestinationExpertTeamSummary,
                Phone = a.DestinationExpertTeamPhone,
                Description = a.DestinationExpertTeamContactDescription,
                RHSText = a.DestinationExpertTeamContactRHSText,
                Url = $"{PathConstants.DESTINATION_EXPERTS}/{a.NodeAlias}",
                Destinations = a.DestinationExpertTeamDestinations,
                FavouriteTourCode = a.DestinationExpertTeamFavouriteTourCode
            };
        }

        public static ExperienceSummaryModel CreateExperienceSummaryModel(ETG.Core.PageTypes.Experience a)
        {
            return new ExperienceSummaryModel
            {
                NodeGuid = a.NodeGUID,
                Name = a.ExperienceName,
                Image = a.ExperienceHeroImage,
                ForegroundImage = a.DestinationHeroForegroundImage,
                DisableOverlay = a.DestinationHeroDisableOverlay,
                HeroAltText = a.ExperienceHeroImageAltTag,
                Path = a.NodeAliasPath,
                Summary = a.ExperienceSummary,
                IconClass = a.ExperienceIcon,
                IconDarkImage = a.ExperienceHeadingIconImage,
                IconWhiteImage = a.ExperienceHeadingWhiteIconImage,
                SVGIcon = a.ExperienceSVGIcon
            };
        }

        private static PricingItemModel CreatePriceItem(string city, TourData.FlightClass flightClass, int price)
        {
            return new PricingItemModel
            {
                DepartureCity = city,
                FlighClass = TourData.GetFlightClassName(flightClass),
                Price = price
            };
        }

        private static void GetPricings(string cities, int economyPrice, int premiumPrice,
            int businessClassPrice, int firstClassPrice, List<PricingItemModel> pricingItems)
        {
            if (cities.IsNullOrEmpty() || (economyPrice == 0 && premiumPrice == 0 && businessClassPrice == 0 &&
                                           firstClassPrice == 0))
            {
                return;
            }

            if (pricingItems == null) pricingItems = new List<PricingItemModel>();

            var arrCity = cities.Split('|');

            foreach (var city in arrCity)
            {
                if (economyPrice > 0)
                {
                    pricingItems.Add(CreatePriceItem(city, TourData.FlightClass.ECONOMY, economyPrice));
                }

                if (premiumPrice > 0)
                {
                    pricingItems.Add(CreatePriceItem(city, TourData.FlightClass.PREMIUM, premiumPrice));
                }

                if (businessClassPrice > 0)
                {
                    pricingItems.Add(CreatePriceItem(city, TourData.FlightClass.BUSINESS_CLASS, businessClassPrice));
                }

                if (firstClassPrice > 0)
                {
                    pricingItems.Add(CreatePriceItem(city, TourData.FlightClass.FIRST_CLASS, firstClassPrice));
                }
            }
        }

        public static PricingModel CreatePricingModel(Core.PageTypes.TourPricing a)
        {
            var priceModel = new PricingModel
            {
                TourDate = a.TourPricingDate,
                NodeAliasPath = a.NodeAliasPath,
                OverridePrice = a.TourPricingOverridePrice
            };
            if (a.TourPricingOverridePrice > 0)
            {
                return priceModel;
            }

            var pricingItems = new List<PricingItemModel>();

            GetPricings(a.TourPricingDepartureCities1, a.TourPricingEconomyPrice1, a.TourPricingPremiumPrice1,
                a.TourPricingBusinessPrice1, a.TourPricingFirstClassPrice1, pricingItems);
            GetPricings(a.TourPricingDepartureCities2, a.TourPricingEconomyPrice2, a.TourPricingPremiumPrice2,
                a.TourPricingBusinessPrice2, a.TourPricingFirstClassPrice2, pricingItems);
            GetPricings(a.TourPricingDepartureCities3, a.TourPricingEconomyPrice3, a.TourPricingPremiumPrice3,
                a.TourPricingBusinessPrice3, a.TourPricingFirstClassPrice3, pricingItems);
            GetPricings(a.TourPricingDepartureCities4, a.TourPricingEconomyPrice4, a.TourPricingPremiumPrice4,
                a.TourPricingBusinessPrice4, a.TourPricingFirstClassPrice4, pricingItems);

            priceModel.PricingList = pricingItems;

            return priceModel;
        }

        public static DiscountAndOfferModel CreateDiscountAndOfferModel(Core.PageTypes.DiscountAndOffer a)
        {
            return new DiscountAndOfferModel
            {
                DiscountAndOfferGuid = a.NodeGUID,
                OfferText = a.DiscountText,
                OfferHeading = a.DiscountHeading,
                ExpiryDate = a.DiscountExpiry,
                DisplayAsLabel = a.DisplayAsOfferLabel,
                DisplayLabel = a.DisplayOfferLabel,
                IsOnSaleNow = a.DiscountIsOnSaleNow,
                FullPaymentRequired = a.DiscountFullPaymentRequired
            };
        }

        public static TravelTypeDetailSummaryModel CreateTravelTypeDetailSummaryModel(
            ETG.Core.PageTypes.TravelTypeDetail a)
        {
            return new TravelTypeDetailSummaryModel
            {
                Name = a.TravelTypeDetailName,
                Path = a.NodeAliasPath,
                PageAlias = a.NodeAlias,
                Heading = a.TravelTypeDetailHeading,
                HeroImage = a.TravelTypeDetailHeroImage,
                HeroIconSvg = a.TravelTypeDetailHeroSVGIcon,
                HeroAltText = a.TravelTypeDetailHeroImageAltTag,
                Summary = a.TravelTypeDetailSummary,
                ThemedPackagesHeading = a.ThemedPackagesHeading,
                ThemedPackagesUrl = a.ThemedPackagesUrl,
                HideGoogleReviews = a.HideGoogleReviews,
                HeroIconClass = a.TravelTypeDetailHeroIcon
            };
        }

        public static TravelTypeLandingModel CreateTravelLandingModel(TravelTypeLanding a)
        {
            return new TravelTypeLandingModel
            {
                DocumentID = a.DocumentID,
                Name = a.TravelTypeName,
                Heading = a.TravelTypeHeading,
                Intro = a.TravelTypeIntro,
                Summary = a.TravelTypeSummary,
                PageHero = new PageHeroModel
                {
                    Heading = a.TravelTypeHeading,
                    HeroImage = a.TravelTypeHeroImage,
                    HeroImageAltText = a.TravelTypeHeroAltText,
                    HeroIconImage = a.TravelTypeHeroIcon,
                    HeroIconSvg = a.TravelTypeSVGIcon
                },

                Page = new PageNodeModel
                {
                    DocumentID = a.DocumentID,
                    PageTitle = a.DocumentPageTitle,
                    PageDescription = a.DocumentPageDescription,
                    PageAliasPath = a.NodeAliasPath,
                    PageAlias = a.NodeAlias,
                    PageKeywords = a.DocumentPageKeyWords,
                    ShareTitle = a.DocumentPageTitle,
                    ShareDescription = a.DocumentPageDescription,
                    ShareImage = a.TravelTypeHeroImage,
                    ExcludedFromSearch = a.DocumentSearchExcluded,
                }
            };
        }
    }
}
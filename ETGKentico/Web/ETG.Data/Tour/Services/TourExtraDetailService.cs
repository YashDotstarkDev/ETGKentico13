using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Castle.Core.Internal;
using CMS.DocumentEngine;
using CMS.EventLog;
using Devotion.Cache;
using ETG.Core.Kentico;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Models.Common;
using ETG.Data.Tour.Models;

namespace ETG.Data.Tour.Services
{
    public class TourExtraDetailService : ITourExtraDetailService
    {
        private readonly ITourService _tourService;
        private readonly ICacheProvider _cacheProvider;
        private readonly ISiteContext _siteContext;
        public TourExtraDetailService(ISiteContext siteContext, ITourService tourService, ICacheProvider cacheProvider)
        {
            _tourService = tourService;
            _cacheProvider = cacheProvider;
            _siteContext = siteContext;
        }
        public List<TourItineraryModel> GetTourItinerary(string path)
        {
            return TourItineraryProvider.GetTourItineraries()
                .OnCurrentSite()
                .Path(path, PathTypeEnum.Section)
                .OrderBy(nameof(TourItinerary.NodeOrder))
                .Select(a => new TourItineraryModel
                {
                    NodeID = a.NodeID,
                    NodeOrder = a.NodeOrder,
                    DayCaption = a.TourItineraryDayCaption,
                    Title = a.TourItineraryTitle,
                    Details = a.TourItineraryDetail,
                    Image = a.TourItineraryImage,
                    ImageCaption = a.TourItineraryImageCaption,
                    Inclusion = a.TourItineraryInclusion,
                    LocationName = a.TourItineraryLocation,
                    LocationSummary = a.TourItinerarySummary,
                    LocationLatitude = a.TourItineraryLatitude,
                    LocationLongitude = a.TourItineraryLongitude,
                    LocationUrl = a.TourItineraryUrl
                }).ToList();
        }

        private List<NameValuePathModel> GetTourHighlightsInternal(string path)
        {
            return TourHighlilghtProvider.GetTourHighlilghts()
                .OnCurrentSite()
                .Columns(nameof(TourHighlilght.TourHighlightIcon), nameof(TourHighlilght.TourHighlightLabel), "NodeAliasPath", "NodeOrder")
                .Path(path, PathTypeEnum.Section)
                .OrderBy(nameof(TourHighlilght.NodeOrder))
                .Select(a =>  new NameValuePathModel
                {
                    Name = a.TourHighlightIcon,
                    Path = a.NodeAliasPath,
                    Value = a.TourHighlightLabel,
                    NodeOrder = a.NodeOrder
                })
                .ToList();
        }
        
        private List<NameValuePathModel> GetTourBonusesInternal(string path)
        {
            return TourBonusProvider.GetTourBonus()
                .OnCurrentSite()
                .Columns(nameof(TourBonus.TourHighlightIcon), nameof(TourBonus.TourHighlightLabel), "NodeAliasPath", "NodeOrder")
                .Path(path, PathTypeEnum.Section)
                .OrderBy(nameof(TourBonus.NodeOrder))
                .Select(a => new NameValuePathModel
                {
                    Name = a.TourHighlightIcon,
                    Path = a.NodeAliasPath,
                    Value = a.TourHighlightLabel,
                    NodeOrder = a.NodeOrder
                })
                .ToList();
        }
        
        public List<NameValuePathModel> GetTourHighlights(string path)
        {
            var allhighlights = GetTourHighlightsInternal(path);
            var allbonuses = GetTourBonusesInternal(path);

            return allhighlights.Where(a => a.Path.ToLower().Contains("/highlights/") || a.Path.ToLower().Contains("/highlight/")).Union(
                    allbonuses.Where(a => a.Path.ToLower().Contains("/highlights/") || a.Path.ToLower().Contains("/highlight/"))).ToList().OrderBy(a => a.NodeOrder)
                .ToList();
        }
        public List<NameValuePathModel> GetTourBonuses(string path)
        {
            var allhighlights = GetTourHighlightsInternal(path);
            var allbonuses = GetTourBonusesInternal(path);
            return allhighlights.Where(a => a.Path.ToLower().Contains("/bonus/")).Union(
                    allbonuses.Where(a => a.Path.ToLower().Contains("/bonus/"))).ToList().OrderBy(a => a.NodeOrder)
                .ToList();
        }

        public List<KeyValuePair<string, string>> GetTourInclusions(string path, bool onlyHighlighted = false)
        {
            var query = TourInclusionProvider.GetTourInclusions()
                .OnCurrentSite()
                .Columns(nameof(TourInclusion.TourInclusionIcon), nameof(TourInclusion.TourInclusionLabel))
                .Path(path, PathTypeEnum.Section);

            if (onlyHighlighted)
            {
                query = query.WhereTrue(nameof(TourInclusion.TourInclusionHighlight));
            }
                
            return query.OrderBy(nameof(TourInclusion.NodeOrder))
                .Select(a => new KeyValuePair<string, string>(a.TourInclusionIcon, a.TourInclusionLabel))
                .ToList();
        }

        public List<HotelModel> GetHotels(string guids)
        {
            if (guids.IsNullOrEmpty())
            {
                return null;
            }

            return HotelProvider.GetHotels()
                .OnCurrentSite()
                .WhereIn(nameof(Hotel.NodeGUID), guids.Split(';'))
                .OrderBy("NodeOrder")
                .Select(
                    a => new HotelModel
                    {
                        NodeGuid = a.NodeGUID,
                        HotelId = a.HotelID,
                        Name = a.HotelName,
                        Images = GetImages(a),
                        Review = a.HotelReview,
                        Detail = a.HotelDetail,
                        Facilities = a.HotelFacilities,
                        Stats = a.HotelStats
                    }).ToList();
        }

        private IEnumerable<KeyValuePair<string, string>> GetImages(Hotel hotel)
        {
            var lst = new List<KeyValuePair<string, string>>();
            if (!hotel.HotelImage.IsNullOrEmpty())
            {
                lst.Add(new KeyValuePair<string, string>(hotel.HotelImage, hotel.HotelCaption));
            }

            if (!hotel.HotelImage2.IsNullOrEmpty())
            {
                lst.Add(new KeyValuePair<string, string>(hotel.HotelImage2, hotel.HotelCaption2));
            }
            
            if (!hotel.HotelImage3.IsNullOrEmpty())
            {
                lst.Add(new KeyValuePair<string, string>(hotel.HotelImage3, hotel.HotelCaption3));
            }
            
            if (!hotel.HotelImage4.IsNullOrEmpty())
            {
                lst.Add(new KeyValuePair<string, string>(hotel.HotelImage4, hotel.HotelCaption4));
            }
            return lst;
        }

        public List<HotelModel> GetHotelsByParentAliasPath(string parentAliasPath)
        {
            var hotelLinks = DocumentLinkProvider.GetDocumentLinks().OnCurrentSite().Path(parentAliasPath, PathTypeEnum.Children)
                .OrderBy("NodeOrder").TypedResult.ToList();


            if (hotelLinks.IsNullOrEmpty())
            {
                return new List<HotelModel>();
            }

            var orderByClause = new StringBuilder();

            for (var i= 0; i < hotelLinks.Count;i++)
            {
                orderByClause.AppendLine($"WHEN '{hotelLinks[i].LinkDocumentGuid}' THEN {i}");
            }
            return HotelProvider.GetHotels().OnCurrentSite().WhereIn(nameof(TreeNode.NodeGUID), hotelLinks.Select(h=>h.LinkDocumentGuid).ToList())
                .OrderBy($"CASE NodeGUID {orderByClause} END")
                .TypedResult.Select(
                a => new HotelModel
                {
                    NodeGuid = a.NodeGUID,
                    HotelId = a.HotelID,
                    Name = a.HotelName,
                    Images = GetImages(a),
                    Review = a.HotelReview,
                    Detail = a.HotelDetail,
                    Facilities = a.HotelFacilities,
                    Stats = a.HotelStats,
                    IsMainHotel = hotelLinks.Where(h=>h.LinkDocumentGuid == a.NodeGUID).Select(h=>h.LinkDocumentIsMain).FirstOrDefault() 
                }).ToList();
        }

        public List<HotelModel> GetHotelsByHotelIds(List<int> hotelIds)
        {
            if (hotelIds.IsNullOrEmpty())
            {
                return null;
            }
            var orderByClause = new StringBuilder();

            for (var i = 0; i < hotelIds.Count; i++)
            {
                orderByClause.AppendLine($"WHEN '{hotelIds[i]}' THEN {i}");
            }
            return HotelProvider.GetHotels().OnCurrentSite()
                .WhereIn(nameof(Hotel.HotelID), hotelIds)
                .OrderBy($"CASE HotelID {orderByClause} END").TypedResult.Select(a => new HotelModel
                {
                    NodeGuid = a.NodeGUID,
                    HotelId = a.HotelID,
                    Name = a.HotelName,
                    Images = GetImages(a),
                    Review = a.HotelReview,
                    Detail = a.HotelDetail,
                    Facilities = a.HotelFacilities,
                    Stats = a.HotelStats
                }).ToList();
        }

        public List<TourFreedomOfChoiceModel> GetTourFreedomOfChoiceOptions(string tourAliasPath)
        {
            var itineraries = GetTourItinerary(tourAliasPath);

            if (itineraries.IsNullOrEmpty())
            {
                return null;
            }
            var choices = FreedomOfChoiceOptionProvider.GetFreedomOfChoiceOptions().OnCurrentSite()
                //.Path(tourAliasPath, PathTypeEnum.Children)
                .WhereIn(nameof(TreeNode.NodeParentID), itineraries.Select(a=>a.NodeID).ToList())
                .ToList();
            var returnList = new List<TourFreedomOfChoiceModel>();
            foreach (var itinerary in itineraries)
            {
                var subchoices = choices.Where(a => a.NodeParentID == itinerary.NodeID).OrderBy(a=>a.NodeOrder).ToList();

                if (subchoices.IsNullOrEmpty())
                {
                    continue;
                }

                returnList.AddRange(subchoices.Select(a=>new TourFreedomOfChoiceModel
                {
                    DayCaption = itinerary.DayCaption,
                    FreedomOfChoiceOptionName = a.OptionName,
                    FreedomOfChoiceDescription = a.OptionDescription,
                    OptionGuid = a.NodeGUID
                }));
            }

            return returnList;
        }

        public List<TourRoomUpgradeModel> GetTourRoomUpgrades(string path)
        {
            return RoomUpgradeProvider.GetRoomUpgrades()
                .OnCurrentSite()
                .Path(path, PathTypeEnum.Section)
                .OrderBy(nameof(RoomUpgrade.RoomUpgradeNumber))
                .Select(a => new TourRoomUpgradeModel
                {
                    Title = a.RoomUpgradeTitle,
                    Image = a.RoomUpgradeImage,
                    Description = a.RoomUpgradeDescription,
                    HotelGuid = a.RoomUpgradeHotelGuid,
                    PriceStatement = a.RoomUpgradePriceStatement
                })
                .ToList();
        }

        public List<TourOptionalExtrasModel> GetTourOptionalExtras(string path)
        {
            return OptionalExtrasProvider.GetOptionalExtras()
                .OnCurrentSite()
                .Path(path, PathTypeEnum.Section)
                .OrderBy(nameof(OptionalExtras.OptionalExtrasNumber))
                .Select(a => new TourOptionalExtrasModel{
                    Title = a.OptionalExtrasTitle,
                    Image = a.OptionalExtrasImage,
                    ImageCaption = a.OptionalExtrasImageCaption,
                    Description = a.OptionalExtrasDescription,
                    Duration = a.OptionalExtrasDuration,
                    PriceStatements = a.OptionalExtrasPriceStatement,
                    CostPerPerson = a.OptionalExtrasPerPersonCost,
                    AvailableFrom = a.OptionalExtrasFromDate,
                    AvailableTo = a.OptionalExtrasToDate,
                    AvailableDaysOfWeek = a.OptionalExtrasDaysOfWeek
                    
                })
                .ToList();
        }
 

        private List<TourPackageUpgradeModel> GetTourUpgradesInternal(string path)
        {
            var tourUpgrades = TourUpdateLinkedDocumentProvider.GetTourUpdateLinkedDocuments().OnCurrentSite().Path(path, PathTypeEnum.Children)
                .OrderBy("NodeOrder").TypedResult.ToList();

            if (tourUpgrades.IsNullOrEmpty())
            {
                return null;
            }

            var tourCodes = tourUpgrades.Select(a => a.TourUpgradesTourCode).ToList();
            var tours = _tourService.GetTiledTours(tourCodes, true);

            if (tours.IsNullOrEmpty())
            {
                return null;
            }

            var sortedTours = new List<TourPackageUpgradeModel>();

            foreach (var tourCode in tourCodes)
            {
                var t = tourUpgrades.Where(a => a.TourUpgradesTourCode == tourCode).FirstOrDefault();

                if (t == null)
                {
                    continue;
                }
                sortedTours.Add(
                    new TourPackageUpgradeModel
                    {
                        UpgradeLabel = t.TourUpgradesLabel,
                        IconClass = t.TourUpgradesIconClass,
                        Tour = tours.FirstOrDefault(a => a.TourCode == tourCode)
                    }
                    );
            }

            return sortedTours;

        }

        public List<TourPackageUpgradeModel> GetTourUpgrades(string path)
        {
            return _cacheProvider.GetCached(() => GetTourUpgradesInternal(path),
                new CacheKeyBuilder(_siteContext.SiteName).Append("GetTourUpgrades").Append(path),
                new DependencyBuilder(_siteContext.SiteName,  new []{TourUpdateLinkedDocument.CLASS_NAME, Core.PageTypes.Tour.CLASS_NAME}).DependsOnAllNodesOfPageType().Build());
        }

        public List<TourSummaryInfoModel> GetRelatedTourTiles(string path)
        {
            return _cacheProvider.GetCached(() => GetRelatedTourTilesInternal(path),
                new CacheKeyBuilder(_siteContext.SiteName).Append("GetRelatedTourTiles").Append(path),
                new DependencyBuilder(_siteContext.SiteName, new[] { GenericTourTile.CLASS_NAME, Core.PageTypes.Tour.CLASS_NAME }).DependsOnAllNodesOfPageType().Build());

        }

        private List<TourSummaryInfoModel> GetRelatedTourTilesInternal(string path)
        {
            if (path.IsNullOrEmpty())
            {
                return null;
            }
            var tiles = GenericTourTileProvider.GetGenericTourTiles().OnCurrentSite().Path(path, PathTypeEnum.Children)
                .OrderBy("NodeOrder").ToList();

            if (tiles.IsNullOrEmpty())
            {
                return null;
            }

            var tourCodes = tiles.OrderBy(a => a.NodeOrder).Select(a => a.TourTileTourCode).ToList();
            var tours = _tourService.GetTiledTours(tourCodes);

            if (tours.IsNullOrEmpty())
            {
                return null;
            }

            var sortedTours = new List<TourSummaryInfoModel>();
            foreach (var tile in tiles)
            {
                var tour = tours.FirstOrDefault(a => a.TourCode == tile.TourTileTourCode);
                if (tour == null)
                {
                    continue;
                }
                tour.CustomHeading = tile.TileHeading;
                sortedTours.Add(tour);
            }

            return sortedTours;

        }
    }
}
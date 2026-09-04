using System;
using ETG.Web.DestinationExpertTeam.Models;
using ETG.Web.Models;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using ETG.Web.Brochure.Models;
using ETG.Web.Destination.Models;
using ETG.Web.Models.Base;
using ETG.Web.Models.Common;
using ETG.Web.Models.Forms;
using ETG.Web.Models.Widgets.FAQWidget;
using ETG.Web.Models.GoogleReviews;
using ETG.WebAPI.Models.Reviews;

namespace ETG.Web.Tour.Models
{
    public class TourPageViewModel : BasePageViewModel, IViewModel
    {
        public TourViewModel TourInfo;

        public ProofPointComponentViewModel ProofPointsComponent { get; set; }
        public ShareLinksViewModel ShareLinks { get; set; }
        public DestinationExpertTeamSummaryViewModel DestinationExpert { get; set; }
        public DestinationSummaryViewModel Destination { get; set; }
        public List<NameValuePathViewModel> Highlights { get; set; }
        public List<NameValuePathViewModel> Bonuses { get; set; }
        public List<KeyValuePair<string, string>> Inclusions { get; set; }
        public List<TourItineraryViewModel> Itineraries { get; set; }
        public List<TourRoomUpgradeViewModel> RoomUpgrades { get; set; }
        public List<TourOptionalExtrasViewModel> OptionalExtras { get; set; }
        public List<HotelViewModel> Hotels { get; set; }

        public TourListingViewModel FeatureTours { get; set; }
        public BrochureViewModel Brochure { get; set; }

        public List<TourPackageUpgradeViewModel> TourUpgrades { get; set; }
        public List<TourSummaryInfoViewModel> TourAlternativeDates { get; set; }
        public List<CTAImageViewModel> InPartnershipImages { get; set; }
        public string PeaceOfMindUrl { get; set; }
        public string SafeTravelUrl { get; set; }
        public string FreedomOfChoiceUrl { get; set; }
        public List<FAQViewModel> FAQs { get; set; }
        public List<string> FreedomOfChoiceDays { get; set; }
        public string BookNowDisclaimer { get; set; }
        public string BookingNumber { get; set; }
        public int EntireFlexAmountPerPerson { get; set; }
        public string FAQSchema { get; set; }
        public string ECommerceDataLayerScript { get; set; }
        public string PrintHotelIds { get; set; }
        public string PrintNights { get; set; }
        public DateTime HotelPDFStartDate { get; set; }
        public ReviewListingResponse GoogleReview { get; set; }

        public string HotelPDFStartDateDisplay
        {
            get
            {
                if (HotelPDFStartDate == DateTime.MinValue)
                {
                    return string.Empty;
                }

                return HotelPDFStartDate.ToString("dd MMMM yyyy");
            }
        }

        public List<MapItemViewModel> MapItems
        {
            get
            {
                if (Itineraries.IsNullOrEmpty())
                {
                    return null;
                }

                return Itineraries.Where(a => a.LocationLatitude > 0 && a.LocationLongitude > 0)
                    .Select(a => new MapItemViewModel
                    {
                        Name = a.LocationName,
                        Summary = a.LocationSummary,
                        Latitude = a.LocationLatitude,
                        Longitude = a.LocationLongitude,
                        Url = a.LocationUrl
                    }).ToList();
            }
        }

        public bool HasSectionAdditionalInformation =>
            Destination == null ? !TourInfo.ImportantNote
                                      .IsNullOrEmpty() ||
                                  !TourInfo.Exclusions.IsNullOrEmpty() :
                
            !Destination.OfficialLanguages.IsNullOrEmpty() || 
            !Destination.Currency.IsNullOrEmpty() || 
            !TourInfo.ImportantNote.IsNullOrEmpty() ||
            !TourInfo.Exclusions.IsNullOrEmpty();

        public bool HasHeroBadge =>
            TourInfo.TourSummaryInfo.HasPeaceOfMindGuarantee ||
            TourInfo.TourSummaryInfo.TourHasFreedomOfChoice ||
            TourInfo.TourSummaryInfo.TourIsExclusive ||
            TourInfo.TourSummaryInfo.TourHasSafeTravel;

        public ConnectWithUsViewModel ConnectWithUs { get; set; }
    }
}
using ETG.Data.DestinationExpertTeam.Models;
using ETG.Data.Tour.Models;
using System.Collections.Generic;
using Devotion.Automapper.Common;
using ETG.Data.Brochure.Models;
using ETG.Data.Destination.Models;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;

namespace ETG.Data.Tour.Models
{
    public class TourPageModel : BasePageModel, IDataModel
    {
        public TourModel TourInfo;
        public ShareLinksModel ShareLinks { get; set; }
        public List<NameValuePathModel> Highlights { get; set; }
        public List<NameValuePathModel> Bonuses { get; set; }
        public List<KeyValuePair<string, string>> Inclusions { get; set; }
        public List<TourItineraryModel> Itineraries { get; set; }
        public List<TourRoomUpgradeModel> RoomUpgrades { get; set; }
        public List<TourOptionalExtrasModel> OptionalExtras { get; set; }
        public List<HotelModel> Hotels { get; set; }
        public DestinationExpertTeamSummaryModel DestinationExpert { get; set; }
        public DestinationSummaryModel Destination { get; set; }
        public TourListingModel FeatureTours { get; set; }
        public BrochureModel Brochure { get; set; }
        public List<CTAImageModel> InPartnershipImages { get; set; }
        public List<TourPackageUpgradeModel> TourUpgrades { get; set; }
        public List<TourSummaryInfoModel> TourAlternativeDates { get; set; }
        public string PeaceOfMindUrl { get; set; }
        public string SafeTravelUrl { get; set; }
        public string FreedomOfChoiceUrl { get; set; }
        public List<FAQModel> FAQs { get; set; }
        public List<string> FreedomOfChoiceDays { get; set; }
        public string BookNowDisclaimer { get; set; }

        public ProofPointComponentModel ProofPointsComponent { get; set;}

    }
}
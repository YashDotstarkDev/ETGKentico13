using System;
using Devotion.Automapper.Common;

namespace ETG.Data.Destination.Models
{
    public class DestinationSummaryModel : IDataModel
    {
        public Guid DestinationGuid { get; set; }
        public string HeroImage { get; set; }
        public string HeroForegroundImage { get; set; }
        public bool DisableOverlay { get; set; }
        public string HeroAltText { get; set; }
        public string CampaignTitle { get; set; }
        public string Name { get; set; }
        public string Heading { get; set; }
        public string Summary { get; set; }
        public string Path { get; set; }
        public string PageAlias { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsMainDestination { get; set; }
        public string EthnicGroup { get; set; }
        public string OfficialLanguages { get; set; }
        public string Currency { get; set; }
        public string Culture { get; set; }
        public string Geography { get; set; }
        public string EnquiryNotificationEmail { get; set; }
        public string IDInMap { get; set; }
    }
}

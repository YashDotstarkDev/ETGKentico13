using System;
using ETG.Web.Models;

namespace ETG.Web.DestinationExpertTeam.Models
{
    public class DestinationExpertTeamSummaryViewModel : IViewModel
    {
        public string Name { get; set; }
        public string SummaryText { get; set; }

        public string HeroImage { get; set; }
        public string HeroImageAltText { get; set; }
        public string Description { get; set; }

        public string Phone { get; set; }
        public string RHSText { get; set; }
        public string Url { get; set; }
        public string Destinations { get; set; }
        public bool HideCTAButton { get; set; }
    }
}

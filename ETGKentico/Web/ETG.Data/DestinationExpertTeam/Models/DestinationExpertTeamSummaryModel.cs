using System;
using Devotion.Automapper.Common;

namespace ETG.Data.DestinationExpertTeam.Models
{
    public class DestinationExpertTeamSummaryModel : IDataModel
    {
        public string Name { get; set; }
        public string SummaryText { get; set; }

        public string HeroImage { get; set; }
        public string HeroAltText { get; set; }
        public string Description { get; set; }

        public string Phone { get; set; }
        public string RHSText { get; set; }
        public string Url { get; set; }
        public Guid DestinationExpertTeamGuid { get; set; }

        public string FavouriteTourCode { get; set; }
        public string Destinations { get; set; }
        
    }
}

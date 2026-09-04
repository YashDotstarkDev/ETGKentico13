using System;
using System.Collections.Generic;
using ETG.Data.Models.Base;
using ETG.Data.Models.Competition;

namespace ETG.Data.Models.PageTypes
{
    public class HomepageModel : PageNodeModel
    {
        public string HeroImage { get; set; }
        public string HeroVideo { get; set; }
        public string HeroVideoImage { get; set; }
        public string HeroCaption { get; set; }
        public string HeroSubCaption { get; set; }
        public string HeroSlideShowFolder { get; set; }
        public string FeaturedTilesTitle { get; set; }
        public string FeaturedTilesFolder { get; set; }
        public string ProofPointsTitle { get; set; }
        public string ProofPointsFolder { get; set; }
        public string DestinationsSectionTitle { get; set; }
        public Guid DestinationExpertGuid { get; set; }
        public string FeatureTourTitle { get; set; }
        public string FeatureTourCodes { get; set; }
        public string FeatureTourViewAllUrl { get; set; }
        public string FeatureTourTitle2 { get; set; }
        public string FeatureTourCodes2 { get; set; }
        public string FeatureTourViewAllButtonLabel2 { get; set; }
        public string FeatureTourViewAllUrl2 { get; set; }
        public PopupCompetitionModel PopupCompetition { get; set; }
        
        public string TravelBlogsSectionTitle { get; set; }
        public string TravelBlogsSectionDescription { get; set; }
        public string TravelBlogsSectionViewAllUrl { get; set; }
        public string JsonSchema { get; set; }

        public List<Guid> SelectedExperiences { get; set; }
    }
}

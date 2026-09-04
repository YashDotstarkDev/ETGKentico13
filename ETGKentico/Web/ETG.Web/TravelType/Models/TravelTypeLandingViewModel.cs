using System.Collections.Generic;
using ETG.Data.TravelType.Models;
using ETG.Web.Models;
using ETG.Web.Models.Base;
using ETG.Web.Models.Common;

namespace ETG.Web.TravelType.Models
{
    public class TravelTypeLandingViewModel : BasePageViewModel, IViewModel
    {
        public int DocumentID { get; set; }
        public string Name { get; set; }
        public string Heading { get; set; }
        public string Intro { get; set; }
        public string Summary { get; set; }

        public string ListHeading { get; set; }
        
        public PageNodeViewModel Page { get; set; }
        public PageHeroViewModel PageHero { get; set; }
        public List<TravelTypeDetailSummaryModel> TravelTypeDetails { get; set; }
    }
}
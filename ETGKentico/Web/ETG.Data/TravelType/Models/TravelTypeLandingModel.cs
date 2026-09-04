using System.Collections.Generic;
using Devotion.Automapper.Common;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;

namespace ETG.Data.TravelType.Models
{
    public class TravelTypeLandingModel : BasePageModel, IDataModel
    {
        public int DocumentID { get; set; }
        public string Name { get; set; }
        public string Heading { get; set; }
        public string Intro { get; set; }
        public string Summary { get; set; }

        public PageNodeModel Page { get; set; }
        public PageHeroModel PageHero { get; set; }
        public IEnumerable<TravelTypeDetailSummaryModel> TravelTypeDetails { get; set; }
    }
}
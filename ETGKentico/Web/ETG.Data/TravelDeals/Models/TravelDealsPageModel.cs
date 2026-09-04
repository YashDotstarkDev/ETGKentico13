using System;
using System.Collections.Generic;
using Devotion.Automapper.Common;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Models.PageTypes;

namespace ETG.Data.TravelDeals.Models
{
    public class TravelDealsPageModel : BasePageModel, IDataModel
    {
        public PageItemModel Page { get; set; }
        public string TourIndexName { get; set; }
    }
}

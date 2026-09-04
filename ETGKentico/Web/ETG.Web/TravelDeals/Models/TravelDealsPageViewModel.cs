using ETG.Web.Models.Base;
using ETG.Web.Models.Common;
using ETG.Web.Models.PageTypes;
using System;
using System.Collections.Generic;
using ETG.Web.Models;

namespace ETG.Web.TravelDeals.Models
{
    public class TravelDealsPageViewModel : BasePageViewModel, IViewModel
    {
        public PageItemViewModel Page { get; set; }
        public string TourIndexName { get; set; }


    }
}

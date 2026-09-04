using System.Collections.Generic;
using System.Web.Mvc;
using Devotion.Automapper.Common;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;

namespace ETG.Data.Tour.Models
{
    public class TourEnquirePageModel : BasePageModel, IDataModel
    {
        public TourModel TourInfo { get; set; }
        public TourEnquireFormModel Form { get; set; }
    }
}
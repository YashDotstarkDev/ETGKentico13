using System.Collections.Generic;
using System.Web.Mvc;
using Devotion.Automapper.Common;
using ETG.Data.DestinationExpertTeam.Models;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Models.PageTypes;

namespace ETG.Data.Models.Forms
{
    public class GenericEnquiryPageModel : BasePageModel, IDataModel
    {
        public PageItemModel Page { get; set; }
        public GenericEnquiryFormModel Form { get; set; }
        public GenericSideContactModel ContactDetails { get; set; }
        public DestinationExpertTeamSummaryModel DestinationExpertDetails { get; set; }
        public List<SelectListItem> Destinations { get; set; }
        public List<SelectListItem> Experiences { get; set; }
    }
}
using ETG.Web.Models.Base;
using ETG.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Spreadsheet;
using ETG.Web.DestinationExpertTeam.Models;
using ETG.Web.Models.PageTypes;

namespace ETG.Web.Models.Forms
{
    public class GenericEnquiryPageViewModel : BasePageViewModel, IViewModel
    {
        public PageItemViewModel Page { get; set; }
        public GenericEnquiryFormViewModel Form { get; set; }
        public GenericSideContactViewModel ContactDetails { get; set; }
        public DestinationExpertTeamSummaryViewModel DestinationExpertDetails { get; set; }
        public List<SelectListItem> Destinations { get; set; }
        public List<SelectListItem> Experiences { get; set; }
    }
}
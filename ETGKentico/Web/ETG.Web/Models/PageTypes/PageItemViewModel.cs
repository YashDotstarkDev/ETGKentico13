using System.Collections.Generic;
using System.Web.Mvc;
using ETG.Web.Models.Base;
using ETG.Web.Models.Common;
using ETG.Web.Models.Forms;

namespace ETG.Web.Models.PageTypes
{
    public class PageItemViewModel : BasePageViewModel, IViewModel
    {
        public PageNodeViewModel Page { get; set; }
        public PageHeroViewModel PageHero { get; set; }
        public ProofPointComponentViewModel ProofPointsComponent { get; set; }
        public string PageName { get; set; }
        public string Content { get; set; }
        public bool IsNewLandingPage { get; set; }
        public bool FormEnabled { get; set; }
        public string FormTitle { get; set; }
        public GenericEnquiryFormViewModel Form { get; set; }
        public List<SelectListItem> Destinations { get; set; }
        public List<SelectListItem> Experiences { get; set; }
        public string RedirectTo { get; set; }
        public string JsonSchema { get; set; }
    }
}

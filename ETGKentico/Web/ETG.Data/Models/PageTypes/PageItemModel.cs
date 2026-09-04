using System.Collections.Generic;
using System.Web.Mvc;
using Devotion.Automapper.Common;
using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using ETG.Data.Models.Forms;

namespace ETG.Data.Models.PageTypes
{
    public class PageItemModel : BasePageModel, IDataModel
    {
        public PageNodeModel Page { get; set; }
        public PageHeroModel PageHero { get; set; }
        public ProofPointComponentModel ProofPointsComponent { get; set; }
        public string PageName { get; set; }
        public string Content { get; set; }
        public bool IsNewLandingPage { get; set; }
        public bool FormEnabled { get; set; }
        public string FormTitle { get; set; }
        public GenericEnquiryFormModel Form { get; set; }
        public List<SelectListItem> Destinations { get; set; }
        public List<SelectListItem> Experiences { get; set; }
        public string RedirectTo { get; set; }
        public string JsonSchema { get; set; }
    }
}

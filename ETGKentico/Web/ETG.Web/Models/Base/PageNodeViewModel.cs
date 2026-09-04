using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Web.Models.Base
{
    public class PageNodeViewModel : IViewModel
    {
        public int DocumentID { get; set; }
        public Guid NodeGuid { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageKeywords { get; set; }
        public string PageAliasPath { get; set; }
        public string PageAlias { get; set; }
        public string ShareTitle { get; set; }
        public string ShareDescription { get; set; }
        public string ShareImage { get; set; }
        public bool ExcludedFromSearch { get; set; }
    }
}
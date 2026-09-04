using Devotion.Automapper.Common;
using System;

namespace ETG.Data.Models.Base
{
    public class PageNodeModel : IDataModel
    {
        public int DocumentID { get; set; }
        public Guid NodeGuid { get; set; }
        public string PageTitle { get; set; }

        public string PageDescription { get; set; }

        public string PageKeywords { get; set; }
        public string PageAlias { get; set; }

        public string PageAliasPath { get; set; }
        public string ShareTitle { get; set; }
        public string ShareDescription { get; set; }
        public string ShareImage { get; set; }
        public bool ExcludedFromSearch { get; set; }
    }
}

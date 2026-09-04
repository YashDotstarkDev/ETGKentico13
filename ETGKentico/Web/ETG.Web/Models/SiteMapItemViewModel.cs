using System;

namespace ETG.Web.Models
{
    public class SiteMapItemViewModel : IViewModel
    {
        public string Url { get; set; }
        public DateTime DateModified { get; set; }
    }
}
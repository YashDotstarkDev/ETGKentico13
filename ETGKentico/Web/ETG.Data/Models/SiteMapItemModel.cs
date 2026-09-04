using Devotion.Automapper.Common;
using System;

namespace ETG.Data.Models
{
    public class SiteMapItemModel : IDataModel
    {
        public string Url { get; set; }
        public DateTime DateModified { get; set; }
    }
}
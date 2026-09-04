using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Web.Models.Widgets.SmallTilesWidget
{
    public class ImageTileCtaViewModel : IViewModel
    {
        public string Caption { get; set; }
        public string SubCaption { get; set; }
        public string ImagePath { get; set; }
        public string Url { get; set; }
        public string CtaLabel { get; set; }
    }
}
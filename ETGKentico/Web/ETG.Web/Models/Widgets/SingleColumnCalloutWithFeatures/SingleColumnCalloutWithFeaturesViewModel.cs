using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using System.Collections.Generic;

namespace ETG.Web.Models.Widgets.SingleColumnCalloutWithFeatures
{
    public class SingleColumnCalloutWithFeaturesViewModel
    {
        public string Heading { get; set; }
        public string CalloutHeading { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public List<string> Features { get; set; }

    }
}
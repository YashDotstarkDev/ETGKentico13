using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Castle.Core.Internal;
using ETG.Data.Tour.Models;
using ETG.Data.Tour.Services;
using ETG.Web.Tour.Models;

namespace ETG.Web.Models.Widgets.ProductsWidget
{
    public class ProductsWidgetViewModel
    {
        public string Heading { get; set; }
        
        public string TourCodes { get; set; }
        
        public string TourType { get; set; }
        
        public string CruiseType { get; set; }
        public string SectionId { get; set; }
        public TourListingViewModel Tours { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using CMS.Ecommerce;
using ETG.Web.Models;
using ETG.Web.Models.Base;
using ETG.Web.Models.Common;

namespace ETG.Web.Tour.Models
{
    public class TourEnquirePageViewModel : BasePageViewModel, IViewModel
    {

        public TourViewModel TourInfo { get; set; }
        public TourEnquireFormViewModel Form { get; set; }


        public List<SelectListItem> PricingClasses
        {
            get
            {
                var selectItems = new List<SelectListItem>();

                    var item = new SelectListItem();
                    item.Value = string.Empty;
                    item.Text = "Please select";
                    selectItems.Add(item);

                    return selectItems;
            }
        }

        public List<SelectListItem> PricingCities {
            get
            {
                var selectItems = new List<SelectListItem>();

                var item = new SelectListItem();
                item.Value = string.Empty;
                item.Text = "Please select";
                selectItems.Add(item);

                return selectItems;
            }
        }
    }
}
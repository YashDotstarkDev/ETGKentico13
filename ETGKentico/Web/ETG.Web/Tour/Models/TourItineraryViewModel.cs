using ETG.Web.Models;
using System;
using Castle.Core.Internal;
using System.Collections.Generic;

namespace ETG.Web.Tour.Models
{
    public class TourItineraryViewModel : IViewModel
    {
        public int NodeID { get; set; }
        public int NodeOrder { get; set; }
        public string Title { get; set; }
        public string DayCaption { get; set; }

        public string DayCaptionFormatted => DayCaption.IsNullOrEmpty() ? string.Empty : $"{DayCaption.Replace("Day", "<span>Day</span>")}";
        public string TitleFormatted => Title.IsNullOrEmpty() ? string.Empty : $"{Title.Replace("→", @"<span class=""arrow fal fa-arrow-right""></span>")}";

        public string Details { get; set; }
        public string Image { get; set; }
        public string Inclusion { get; set; }

        public string InclusionDisplay
        {
            get
            {
                if (Inclusion.IsNullOrEmpty())
                {
                    return string.Empty;
                }

                return Inclusion.Replace(",", ", ");
            }
        }


        public string LocationUrl { get; set; }
        public string ImageCaption { get; set; }
        public string LocationName { get; set; }
        public string LocationSummary { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        public List<TourFreedomOfChoiceViewModel> FreedomOfChoices { get; set; }
    }
}

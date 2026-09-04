using ETG.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using ETG.Data.Models.Booking;
using ETG.Module.Booking.Models.Cart;
using ETG.Web.Experience.Models;
using ETG.Web.Models.Common;

namespace ETG.Web.Tour.Models
{
    public class TourSummaryInfoViewModel : IViewModel
    {
        public IEnumerable<ImageViewModel> Images { get; set; }

        /*public string HeroImage { get; set; }
        public string HeroImageAltText { get; set; }
        public string HeroImageCaption { get; set; }*/
        public string Name { get; set; }
        public string TourCode { get; set; }
        public string DepartureCity { get; set; }

        public string DestinationCity { get; set; }

        public ImageViewModel HeroImage
        {
            get
            {
                if (Images.IsNullOrEmpty())
                {
                    return null;
                }

                return Images.FirstOrDefault();
            }
        }
        
        public ImageViewModel ImageAt(int index)
        {
            
            if (Images.IsNullOrEmpty())
            {
                return null;
            }

            return Images.Skip(index).Take(1).FirstOrDefault();
            
        }

        public string DepartAndDestinationCity(string delimeter)
        {
            if (DepartureCity.IsNullOrEmpty() && DestinationCity.IsNullOrEmpty())
            {
                return string.Empty;
            }

            if (!DepartureCity.IsNullOrEmpty() && !DestinationCity.IsNullOrEmpty())
            {
                return $"{DepartureCity}{delimeter}{DestinationCity}";
            }

            if (!DestinationCity.IsNullOrEmpty())
            {
                return DestinationCity;
            }

            return DepartureCity;
        }
        
        public string PrimaryCountryName { get; set; }
        public IEnumerable<string> SubCountryNames { get; set; }
        public string DisplayedCountryNames
        {
            get
            {
                if (SubCountryNames?.Any() ?? false)
                {
                    return $"{PrimaryCountryName} + {string.Join(" + ", SubCountryNames)}";
                }

                return PrimaryCountryName;
            }
        }
        
        public int NoOfNights { get; set; }
        public PricingViewModel FromPrice { get; set; }
        public string PriceCurrencySymbol => "$";
        public string PriceTypeLabel { get; set; }

        public string PriceTypeLabelFormatted
        {
            get
            {
                if (PriceTypeLabel.IsNullOrEmpty())
                {
                    return string.Empty;
                }

                var label = PriceTypeLabel.Replace(",", "");
                if (label.Length == 1)
                {
                    return label;
                }
                return $"{label[0].ToString().ToUpper()}{label.Substring(1)}";
            }
        }

        public string Summary { get; set; }

        public string SummaryFirst100Words
        {
            get
            {
                if (Summary.IsNullOrEmpty())
                {
                    return string.Empty;
                }

                
                var arr = Summary.Replace("\"", "").Split(' ');

                if (arr.Length > 100)
                {
                    return string.Join(" ", arr);
                }
                return Summary;
            }
        }

        public string TypeCodes { get; set; }
        public bool IsCruise { get; set; }
        public bool HasPeaceOfMindGuarantee { get; set; }
        public bool TourHasFreedomOfChoice { get; set; }
        public bool TourIsExclusive { get; set; }
        public bool TourHasSafeTravel { get; set; }
        public int CruiseType { get; set; }
        public string ExperienceGuids { get; set; }
        public bool HideIconIcons { get; set; }
        public List<KeyValuePair<string, string>> TourTypeIcons { get; set; }
        public List<KeyValuePair<string, string>> CruiseTypeIcons { get; set; }
        public List<KeyValuePair<string, string>> ExperienceIcons { get; set; }
        public List<ExperienceSummaryViewModel> Experiences { get; set; }
        public List<TourTypeViewModel> TourTypes { get; set; }
        public string ExperiencesText
        {
            get
            {
                if (ExperienceIcons.IsNullOrEmpty())
                {
                    return string.Empty;
                }
                return string.Join(", ", ExperienceIcons.Select(a => a.Key).OrderBy(a=>a));
            }
        }

        public string TourTypesText
        {
            get
            {
                if (TourTypeIcons.IsNullOrEmpty())
                {
                    return string.Empty;
                }
                return string.Join(", ", TourTypeIcons.Select(a => a.Key).OrderBy(a => a));
            }
        }

        public List<IconSVGViewModel> PriceInclusionIcons { get; set; }
        public Guid DiscountGuid { get; set; }
        public string Path { get; set; }
        public string NodeAliasPath { get; set; }
        public DiscountAndOfferViewModel Discount { get; set; }
        public PromotionItem Promotion { get; set; }
		//public bool HasEntireFlex { get; set; }

        public bool TourIsOnSaleNow
        {
            get
            {
                if (Discount != null)
                {
                    return Discount.IsOnSaleNow;
                }

                return false;
            }
        }
        
        public bool OnSaleFullpaymentRquired
        {
            get
            {
                if (Discount != null)
                {
                    return Discount.FullPaymentRequired;
                }

                return false;
            }
        }
        public string CustomHeading { get; set; }
        public string MainHotelName { get; set; }
        public IEnumerable<string> HighlightedInclusions { get; set; }
        public string TourDepartsFromInAustralia { get; set; }
        public string TourTravelEnds { get; set; }
    }
}

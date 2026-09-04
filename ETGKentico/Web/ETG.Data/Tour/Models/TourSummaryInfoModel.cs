using Devotion.Automapper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Devotion.Web.Base.Extensions;
using ETG.Data.Experience.Models;
using ETG.Data.Models.Booking;
using ETG.Data.Models.Common;
using ETG.Data.Models.Modules;

namespace ETG.Data.Tour.Models
{
    public class TourSummaryInfoModel : IDataModel
    {
        public Guid NodeGuid { get; set; }
        
        public IEnumerable<ImageModel> Images { get; set; }
        
        public string Name { get; set; }
        public string TourCode { get; set; }

        public Guid PrimaryCountryGuid { get; set; }
        public string PrimaryCountryName { get; set; }
        public string SubCountriesGuids { get; set; }
        public IEnumerable<string> SubCountryNames { get; set; }
        public DateTime DateModifed { get; set; }
        public string RelatedTourCodes { get; set; }
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
        public List<Guid> AllDestinationGuids
        {
            get
            {
                var guids = new List<Guid>();

                if (PrimaryCountryGuid != Guid.Empty)
                {
                    guids.Add(PrimaryCountryGuid);
                }

                if (!SubCountriesGuids.IsNullOrEmpty())
                {
                    guids.AddRange(SubCountriesGuids.ToGuidList(';'));

                }
                return guids.Distinct().ToList();

            }
        }

        public string DepartureCity { get; set; }
        public string DestinationCity { get; set; }
        public int NoOfNights { get; set; }
        public PricingModel FromPrice { get; set; }
        public string PriceInclusions { get; set; }
        public string PriceTypeLabel { get; set; }
        public string Summary { get; set; }
        public string TypeCodes { get; set; }
        public bool IsCruise { get; set; }
        public bool HasPeaceOfMindGuarantee { get; set; }
        //public bool HasEntireFlex { get; set; }
        public bool TourHasFreedomOfChoice { get; set; }
        public bool TourIsExclusive { get; set; }
        public bool TourHasSafeTravel { get; set; }
        public int CruiseType { get; set; }
        public string ExperienceGuids { get; set; }
        public Guid PrimaryExperienceGuid { get; set; }
        public bool HideIconIcons { get; set; }
        public List<KeyValuePair<string, string>> TourTypeIcons { get; set; }
        public List<KeyValuePair<string, string>> CruiseTypeIcons { get; set; }
        public List<KeyValuePair<string, string>> ExperienceIcons { get; set; }

        //for edm
        public List<TourTypeModel> TourTypes { get; set; }
        //for edm
        public List<ExperienceSummaryModel> Experiences { get; set; }
        public ExperienceSummaryModel PrimaryExperience { get; set; }
        public List<IconSVGModel> PriceInclusionIcons { get; set; }
        public Guid DiscountGuid { get; set; }
        public string Path { get; set; }
        public string NodeAliasPath { get; set; }
        public DiscountAndOfferModel Discount { get; set; }
        public PromotionItem Promotion { get; set; }
        public string CustomHeading { get; set; }
        public bool DocumentSearchExcluded { get; set; }
        public string MainHotelName { get; set; }
        public IEnumerable<string> HighlightedInclusions { get; set; }
        public string TourDepartsFromInAustralia { get; set; }
        public string TourTravelEnds { get; set; }
    }
}

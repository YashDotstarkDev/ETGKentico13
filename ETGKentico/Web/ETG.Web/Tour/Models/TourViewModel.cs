using ETG.Web.Models.Base;
using ETG.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.Core.Internal;
using ETG.Data.Extensions;
using ETG.Web.Models;
using ETG.Web.Models.Forms;

namespace ETG.Web.Tour.Models
{
    public class TourViewModel : PageNodeViewModel, IViewModel
    {
        public TourSummaryInfoViewModel TourSummaryInfo { get; set; }
        public string Details { get; set; }
        public string MapImage { get; set; }
        public string Exclusions { get; set; }
        public string AdditionalInclusions { get; set; }
        public string DepartFromInAustralia { get; set; }
        public string TravelEndsLocation { get; set; }
        public string DestinationsText { get; set; }
        public string TravelDates { get; set; }

        public Guid PrimaryCountryGuid { get; set; }
        public string SubCountriesGuids { get; set; }
        public string OptionalExtras { get; set; }
        public string ImportantNote { get; set; }
        public string Smallprints { get; set; }
        public string HotelGuids { get; set; }
        public bool IsPublished { get; set; }
        public bool HasPeaceOfMindGuarantee { get; set; }
        public bool HasSafeTravel { get; set; }
        public bool HasFreedomOfChoice { get; set; }
        public bool BookNowEnabled { get; set; }
        //public bool TourFOCEntireFlex { get; set; }
        public int BookingDepartureDatesOptions { get; set; }
        public DateTime BookingEarliestDepartureDate { get; set; }
        public DateTime BookingLatestDepartureDate { get; set; }
        public string PaymentTerms { get; set; }
        public int FullPaymentDaysFromDepartureDate { get; set; }
        public List<int> DepartureDaysOfWeek { get; set; }
        public int SkuId { get; set; }

        public string TourTypeDescription { get; set; }
        public string FreedomOfChoiceDescription { get; set; }
        public string SafeTravelDescription { get; set; }
        public string ExclusivePackageDescription { get; set; }
        public List<string> PeaceOfMindCheckList { get; set; }
        public double SaleComissionPercent { get; set; }

    }
}
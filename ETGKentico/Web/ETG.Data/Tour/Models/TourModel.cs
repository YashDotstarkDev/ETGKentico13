using ETG.Data.Models.Base;
using ETG.Data.Models.Common;
using System;
using System.Collections.Generic;

namespace ETG.Data.Tour.Models
{
    public class TourModel : PageNodeModel
    {
        public TourSummaryInfoModel TourSummaryInfo { get; set; }
        public string Details { get; set; }
        public string MapImage { get; set; }

        public string AdditionalInclusions { get; set; }
        public string Exclusions { get; set; }
        public string DepartFromInAustralia { get; set; }
        public string TravelEndsLocation { get; set; }
        public string DestinationsText { get; set; }
        public string TravelDates { get; set; }
        public string OptionalExtras { get; set; }
        public string ImportantNote { get; set; }
        public string Smallprints { get; set; }
        public string HotelGuids { get; set; }

        public bool IsPublished { get; set; }
        public string InPartnershipPath { get; set; }
        public bool HasPeaceOfMindGuarantee { get; set; }
        public bool HasSafeTravel { get; set; }
        public bool HasFreedomOfChoice { get; set; }
        public bool BookNowEnabled { get; set; }
        //public bool TourFOCEntireFlex { get; set; }
        public int BookingDepartureDatesOptions { get; set; } 
        public DateTime BookingEarliestDepartureDate { get; set; }
        public DateTime BookingLatestDepartureDate { get; set; }
        public string PaymentTerms { get; set; }
        public bool IsFullPayment { get; set; }
        public int FullPaymentDaysFromDepartureDate { get; set; }
        public int DepositValue { get; set; }
        public double DepositPercentage { get; set; }
        public List<int> DepartureDaysOfWeek { get; set; }
        public int SkuId { get; set; }
        public string AlternativeDatesPath { get; internal set; }
        public int ChangeOfMindThresholdDays { get; set; }
        public double SaleComissionPercent { get; set; }

        public string FaqsFolderPath { get; set; }
    }
}

using System;
using System.Linq;
using System.Text;
using Castle.Core.Internal;
using CMS.Base;
using CMS.Ecommerce;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Constants;
using ETG.Module.Booking.Extensions;

namespace ETG.Module.Booking.Models.Cart
{
    public class DetailedPackageBooking
    {
        public string TourCode { get; set; }
        public string TourName { get; set; }
        public string TourDestination { get; set; }
        public string TourUrl { get; set; }
        public bool IsOnSaleNow { get; set; }
        public bool OnSaleFullPaymentRequired { get; set; }
        public bool TourHasPeaceOfMind { get; set; }
        //public bool IsFOCEntireFlex { get; set; }
        public OrderInfo Order { get; set; }
        public BookingQuoteInfo Quote { get; set; }
        public DateTime DepartureDate { get; set; }
        public int TwinShareRoomCount { get; set; }
        public int SingleRoomCount { get; set; }
        public string TwinShareRoomsType { get; set; }

        public RoomOptionsSummaryCartData RoomOptions { get; set; }
        public ExtrasSummaryCartData Extras { get; set; }

        public FreedomOfChoicesSummaryCartData FreedomOfChoices { get; set; }
        public bool TravelInsuranceAssistance { get; set; }
        public bool InternationalAirfareAssistance { get; set; }
        public DateTime SecondInstalmentDate { get; set; }
        public double SecondInstalmentPercentage { get; set; }
        public DateTime PaymentBalanceDueDate { get; set; }
        public int SourceQuoteID { get; set; }
        public string Currency { get; set; }
        public decimal ConversionRate { get; set; }
        
        public PrePostNightsDetails PreNightsDetails { get; set; }
        public PrePostNightsDetails PostNightsDetails { get; set; }
        public double TwinPreNightBasePrice { get; set; }
        public double TwinPostNightBasePrice { get; set; }
        public double SinglePreNightBasePrice { get; set; }
        public double SinglePostNightBasePrice { get; set; }
        public string PromoCode { get; set; }
        
    }
}

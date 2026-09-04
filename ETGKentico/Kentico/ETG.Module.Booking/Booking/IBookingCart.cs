using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Core;
using CMS.Ecommerce;
using CMS.Helpers;
using ETG.Booking.Pricing.Models;
using ETG.Core.Promotion;
using ETG.Data.Models.Booking;
using ETG.Data.Promotion;
using ETG.Data.Tour.Repositories;
using ETG.Module.Booking.Booking;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Models;
using ETG.Module.Booking.Models.Cart;

namespace ETG.Module.Booking.Shopping
{
    public interface IBookingCart
    {
        void ClearCart(); 
        ContainerCustomData CurrentCartItemData { get; }
        bool CartIsEmpty { get; }
        string CurrentTourCode { get; }
        string SessionID { get; }
        int TotalPersons { get; }
        int TotalRooms { get; }
        int TotalTwinShareRooms { get; }
        int TotalSingleRooms { get; }
        bool CurrentCartComplete { get; }
        DateTime CurrentDepartureDate { get; }
        double GetBookingPriceSingleSupplementPrice();
        string CurrentTourAliasPath { get; }
        bool CurrentTourHasPeaceOfMind { get; }
        bool CurrentTourIsOnSaleNow { get; }
        bool CurrentTourOnSaleFullPaymentRequired { get; }
        bool CurrentCartRequiredFareAssistance { get; }
        bool CurrentCartRequiredInsuranceAssistance { get; }
        bool CurrentCartAvailFlexOption { get; }
        int CurrentTourChangeOfMindThreshold { get; }
        int CurrentTourNumberOfNights { get; }
        bool IsSoloBooking { get;  }
        double GetCartTotalPrice();
        ExtrasSummaryCartData GetSelectedExtras();
        FreedomOfChoicesSummaryCartData GetSelectedFreedomOfChoices();
        BookingAgentDetails GetAgentDetails();
        void AddNonAgentBookingToCart(string sessionId, string tourcode, int skuId, DateTime departureDate, DateBookingPrice dateBookingPrice);

        void AddTravelAgentBookingToCart(string sessionId, string tourCode, int skuId,BookingAgentDetails agentDetails);
        void SetDepartureDateAndPrice(DateTime departureDate, DateBookingPrice dateBookingPrice);
        void SetNoOfRooms(int twinShareRoomCount, int singleRoomCount, string twinShareRoomsType);
        void SetCartComplete(bool cartComplete);
        double CalculateBasicTotalPrice();
        void SetRoomOptions(List<Guid> roomOptionGuids);
        double GetRoomOptionsSubTotal();
        void SetExtras(bool selectExtrasNow, List<Guid> extraOptionGuids);
        double GetExtrasSubTotal();
        void SetOtherOptions(bool requireFareAssistance, bool requireTravelInsuranceAssistance);
        void SetShoppingCartNote(string note);
        string GetShoppingCartNote();
        void ClearEntireFlex();
        double SetEntireFlexOption(bool requestAvailEntireFlexOption);
        CartBookingSummary GetBookingSummary(int quoteId = 0);
        OrderInfo CreateOrder(CustomerInfo customer);
        double GetEntireFlexDueAmount();
        void SetAgentDetails(BookingAgentDetails form);
        void SetFreedomOfChoices(bool selectNow, List<FreedomOfChoiceItem> freedomOfChoices);
        void SetBookNowShoppingCartNotes(string customerComments);
        //bool CheckIfEntireFlexIsOffered();
        void AddQuoteToCart(BookingQuoteInfo quote);        
        void AddPromotionForAgent(string agentEmail);
        PromotionItem GetPromotion();
        void SetPromotion(PromotionInfo promotion);
        void AddAgentPrice(double price);
        double GetAgentPrice();
        void SetPrePostNights(int preNights, int postNights);
        PrePostNightsDetails GetPreNightsDetails();
        PrePostNightsDetails GetPostNightsDetails();
        PrePostNightsPriceContainer GetPrePostNightsBasePrices();
        string GetPrePostNightsDescription();
        
        string CurrentCurrency { get; }
        decimal ConversionRate { get; }
        void SetPromoCode(string promoCode);
    }
}

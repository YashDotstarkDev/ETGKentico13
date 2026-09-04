using System;
using System.Collections.Generic;
using System.Text;
using Castle.Core.Internal;
using CMS.Ecommerce;
using ETG.Data.Extensions;
using ETG.Data.Settings.Models;
using ETG.Data.Tour;
using ETG.Module.Booking.Booking;
using ETG.Web.Models.Pages;

namespace ETG.Web.Models.Forms
{
    public class BookingCheckoutFormViewModel
    {
        public enum RoomType
        {
            TWINDOUBLE,
            SINGLE,
            ROOMUPGRADE
        }
        public BookingCheckoutPageContentsViewModel PageContents { get; set; }
        public BookingCustomerViewModel CustomerForm { get; set; }
        public CartBookingSummary BookingSummary { get; set; }
        public ETGSettings ETGSettings { get; set; }
        public string TourUrl { get; set; }
        public bool IsAgent { get; set; }
        public bool IsFromQuote { get; set; }
        public int QuoteID { get; set; }

        public string PaymentTermsCopy
        {
            get;
            set;
        }

        public string DueNowCopy
        {
            get
            {
                if (BookingSummary == null || PageContents == null || (BookingSummary.IsOnSaleNow && BookingSummary.OnSaleFullPaymentRequired))
                {
                    return string.Empty;
                }

                if (!BookingSummary.TourHasPeaceOfMind)
                {
                    return PageContents.DueCopyWithoutPeaceOfMind;
                }

                //if (BookingSummary.EntireFlexPricing == null || BookingSummary.EntireFlexPricing.Count == 0)
                //{
                    return PageContents.DueCopyWithPeaceOfMind;
                //}

                //return PageContents.DueCopyWithPeaceOfMindAndFlex;

            }
        }

        public string ErrorMessage { get; internal set; }
        public bool HasPeaceOfMindGuarantee { get; set; }
        public List<string> PeaceOfMindCheckList { get; set; }
        public CurrentCurrencyPricing CurrentCurrencyPricing { get; set; }
        public string NewCurrencyCookie { get; set; }

        public string PrePostNightsBreakdown
        {
            get
            {
                if ( BookingSummary == null || (BookingSummary.PreNightsDetails == null && BookingSummary.PostNightsDetails == null))
                {
                    return string.Empty;
                }

                var html = new StringBuilder("<br>");
                if (BookingSummary.PreNightsDetails != null)
                {
                    html.Append($"Pre nights: {BookingSummary.PreNightsDetails.DateRangeDisplay}");
                }
                if (BookingSummary.PostNightsDetails != null)
                {
                    if (BookingSummary.PreNightsDetails != null)
                    {
                        html.Append("<br>");
                    }

                    html.Append($"Post nights: {BookingSummary.PostNightsDetails.DateRangeDisplay}");
                }

                return html.ToString();
            }
        }
        
        public string GetPrePostNightsPackageBreakdown(RoomType type)
        {
            if (BookingSummary == null ||
                (BookingSummary.PreNightsDetails == null && BookingSummary.PostNightsDetails == null))
            {
                return string.Empty;
            }
            
            if (type == RoomType.TWINDOUBLE && (BookingSummary.Packages == null || BookingSummary.Packages.Count == 0))
            {
                return string.Empty;
            }
            
            if (type == RoomType.SINGLE && (BookingSummary.SinglePackages == null || BookingSummary.SinglePackages.Count == 0))
            {
                return string.Empty;
            }
            
            if (type == RoomType.ROOMUPGRADE && (BookingSummary.RoomOptions == null || BookingSummary.RoomOptions.Count == 0))
            {
                return string.Empty;
            }

            var html = new StringBuilder();
            if (BookingSummary.PreNightsDetails != null)
            {
                html.Append($"{BookingSummary.PreNightsDetails.NumberOfNights} Pre Nights added ({BookingSummary.PreNightsDetails.DateRangeDisplay})");
            }
            if (BookingSummary.PostNightsDetails != null)
            {
                if (BookingSummary.PreNightsDetails != null)
                {
                    html.Append("<br>");
                }
                html.Append($"{BookingSummary.PostNightsDetails.NumberOfNights} Post Nights added ({BookingSummary.PostNightsDetails.DateRangeDisplay})");
            }

            return html.ToString();
       
        }
    }
}
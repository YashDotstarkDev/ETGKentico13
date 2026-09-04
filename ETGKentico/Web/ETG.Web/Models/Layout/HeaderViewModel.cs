using ETG.Web.Models.Menu;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;

namespace ETG.Web.Models.Layout
{
    public class HeaderViewModel
    {
        public string Destination { get; set; }

        public List<MainMenuViewModel> MainLinks { get; set; }
        public GDPRConsentViewModel GDPRConsent { get; set; }
        public string PeaceOfMindUrl { get; set; }
        public string SafeTravelUrl { get; set; }
        public string FreedomOfChoiceUrl { get; set; }
        public string BookNowUrl { get; set; }
        
        public string TourSearchIndex { get; set; }
        
        public List<string> Currencies { get; set; }
        public string CurrentCurrency { get; set; }
        public decimal ConversionRate { get; set; }
        public string CurrentCurrencySymbol { get; set; }
        public bool CurrencyApplyDiscounts { get; set; }
        public string CurrentCountryCode { get; set; } = "AU";
        public string HeaderPhoneNumber { get; set; }
        public string OnSaleNowUrl => $"/search?{TourSearchIndex}%5Btoggle%5D%5Boptions.onSale%5D=true";
    }
}
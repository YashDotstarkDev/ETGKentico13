using ETG.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.Core.Internal;
using CMS.Core;
using ETG.Booking.Pricing.Enums;
using ETG.Booking.Pricing.Services;
using ETG.Core.Constants;
using ETG.Data.Extensions;
using ETG.Data.Tour;
using ETG.Data.Tour.Models;
using Ninject;

namespace ETG.Web.Tour.Models
{
    public class PricingViewModel : IViewModel
    {
        private readonly CurrentCurrencyPricing _currentCurrencyPricing;
        public PricingViewModel()
        {
            var currencyService = DependencyResolver.Current.GetService<ICurrencyService>();
            _currentCurrencyPricing = new CurrentCurrencyPricing( currencyService);
            Currency = _currentCurrencyPricing.CurrentCurrency;
        }
        
        
        public string NodeAliasPath { get; set; }
        public string TourDate { get; set; }
        public string Currency { get;  }
        public int OverridePrice { get; set; }
        public List<PricingItemViewModel> PricingList { get; set; }

        public int LowestPrice
        {
            get
            {
                if (OverridePrice > 0 || PricingList.IsNullOrEmpty())
                {
                    return OverridePrice;
                }

                return PricingList.Where(a => a.Price > 0).Min(a => a.Price);

            }
        }

        public string DisplayedPrice => $"{CurrencyConstants.CODE_AUD}{LowestPrice:#,###}";
        public string DisplayedPriceNoCurrency=> $"{LowestPrice:#,###}";

        public string GetDisplayPriceForCurrentCurrency()
        {
            if (Currency == CurrencyConstants.CODE_AUD)
            {
                return DisplayedPrice;   
            }

            return _currentCurrencyPricing.GetCurrentCurrencyDisplayPrice(LowestPrice);
        }
        
        public string GetDisplayedPriceNoCurrencyForCurrentCurrency()
        {
            if (Currency == CurrencyConstants.CODE_AUD)
            {
                return DisplayedPriceNoCurrency;   
            }
            
            return $"{_currentCurrencyPricing.ConvertAUDToCurrentCurrency(LowestPrice):#,###}";
        }
    }
}

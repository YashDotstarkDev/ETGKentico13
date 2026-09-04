using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using CMS.Base.Internal;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.SiteProvider;
using Devotion.Cache;
using ETG.Booking.Pricing.Enums;

namespace ETG.Booking.Pricing.Services
{
    public class CurrencyService : ICurrencyService
    {
        private const string CURRENCY_COOKIE = "CurrentCurrency";
        private readonly ICacheProvider _cacheProvider;

        public CurrencyService(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }


        public bool IsValidCurrencyCode(string currencyCode)
        {
            return GetSupportedCurrencies().Contains(currencyCode);
        }

        public void SetCurrentCurrency(string currencyCode)
        {
            CookieHelper.SetValue(new CookieHelperValueSettingParameters
            {
                Name = CURRENCY_COOKIE,
                Value = currencyCode,
                Expires = DateTime.Now.AddYears(1)
            });
        }

        public string GetCurrentCurrency()
        {
            string host = HttpContext.Current.Request.Url.Host;
            var currency = CookieHelper.GetValue(CURRENCY_COOKIE);

            if (currency == null)
            {
                return host.ToLowerInvariant().Contains("nz") ? CurrencyConstants.CODE_NZD : CurrencyConstants.CODE_AUD;
            }

            return currency;
        }

        private IEnumerable<string> GetSupportedCurrenciesInternal()
        {
            return CurrencyInfoProvider.GetCurrencies(SiteContext.CurrentSiteID, true).Select(a=>a.CurrencyCode).ToList();
        }
        
        public IEnumerable<string> GetSupportedCurrencies()
        {
            return _cacheProvider.GetCached(GetSupportedCurrenciesInternal, $"GetSupportedCurrencies",
                CurrencyInfo.OBJECT_TYPE);
        }

        public decimal GetConversionRate(string currencyCode)
        {
            return CurrencyConverter.Convert(1, currencyCode,"AUD" , SiteContext.CurrentSiteID);
        }

        
    }
}
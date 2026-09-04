using System;
using Castle.Core.Internal;
using ETG.Booking.Pricing.Enums;
using ETG.Booking.Pricing.Services;
using ETG.Data.Extensions;

namespace ETG.Data.Tour
{
    public class CurrentCurrencyPricing
    {
        private readonly string _currentCurrency;
        private readonly decimal _currentConversionRate;
        private readonly ICurrencyService _currencyService;
        public CurrentCurrencyPricing(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
            _currentCurrency = currencyService.GetCurrentCurrency();
            _currentConversionRate = currencyService.GetConversionRate(_currentCurrency);
        }

        public CurrentCurrencyPricing(string currency, decimal conversionRate)
        {
            if (currency.IsNullOrEmpty())
            {
                currency = CurrencyConstants.CODE_AUD;
            }
            _currentCurrency = currency;
            _currentConversionRate = conversionRate;
        }

        public string CurrentCurrency => _currentCurrency;
        public string CurrentCurrencySymbol => _currentCurrency.CurrencySymbol();
        public decimal ConversionRate => _currentConversionRate;
        public bool CurrentCurrencyIsAUD => _currentCurrency.Equals(CurrencyConstants.CODE_AUD);
        public bool CurrencyAppliesDiscounts => CurrentCurrency == CurrencyConstants.CODE_AUD || CurrentCurrency == CurrencyConstants.CODE_NZD;
        public int ConvertAUDToCurrentCurrency(int price)
        {
            if (_currentCurrency == CurrencyConstants.CODE_AUD)
            {
                return price;
            }

            return (int) Math.Ceiling(price * _currentConversionRate);
        }
        
        public int ConvertAUDToCurrentCurrency(double price)
        {
            if (_currentCurrency == CurrencyConstants.CODE_AUD)
            {
                return (int)price;
            }

            return (int) Math.Ceiling((decimal )price * _currentConversionRate);
        }
        
        public int ConvertAUDToCurrentCurrency(decimal price)
        {
            if (_currentCurrency == CurrencyConstants.CODE_AUD)
            {
                return (int)price;
            }

            return (int) Math.Ceiling(price * _currentConversionRate);
        }
        
        public int ConvertCurrentCurrencyToAUD(double price)
        {
            if (_currentCurrency.IsNullOrEmpty() || _currentCurrency == CurrencyConstants.CODE_AUD || _currentConversionRate == 0)
            {
                return (int)price;
            }

            return (int) Math.Ceiling((decimal )price / _currentConversionRate);
        }
        
        public string GetCurrentCurrencyDisplayPrice(int price)
        {
            if (_currentCurrency == "AUD")
            {
                return $"{_currentCurrency}{price:#,###}";
            }
            return $"{_currentCurrency} {ConvertAUDToCurrentCurrency(price):#,###}";
        }
        
        public string GetCurrentCurrencyDisplayPrice(double price)
        {
            if (_currentCurrency == "AUD")
            {
                return $"{_currentCurrency}{price:#,###}";
            }
            return $"{_currentCurrency} {ConvertAUDToCurrentCurrency(price):#,###}";
        }
    }
}
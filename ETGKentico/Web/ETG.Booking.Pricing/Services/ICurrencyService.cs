using System.Collections.Generic;

namespace ETG.Booking.Pricing.Services
{
    public interface ICurrencyService
    {
        bool IsValidCurrencyCode(string currencyCode);
        void SetCurrentCurrency(string currencyCode);
        string GetCurrentCurrency();
        IEnumerable<string> GetSupportedCurrencies();
        decimal GetConversionRate(string currencyCode);
    }
}
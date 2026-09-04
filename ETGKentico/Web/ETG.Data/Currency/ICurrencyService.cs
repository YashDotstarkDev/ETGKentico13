using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Currency
{
    public interface ICurrencyService
    {
        int Convert(string sourceCurrency, string targetCurrency, int value);
        void GetAndSaveCurrency(string sourceCurrency, string targetCurrency);
        double GetAdjustedExchangeRate(string sourceCurrency, string targetCurrency);
    }
}

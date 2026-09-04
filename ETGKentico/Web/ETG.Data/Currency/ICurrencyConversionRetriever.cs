using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETG.Module.Models;

namespace ETG.Data.Currency
{
    public interface ICurrencyConversionRetriever
    {
        FixerResult GetLatestCurrencyConverion(string sourceCurrency, string targetCurrency);
    }
}
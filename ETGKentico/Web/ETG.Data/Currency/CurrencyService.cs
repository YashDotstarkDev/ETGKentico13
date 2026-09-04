using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETG.Booking.Pricing.Enums;
using ETG.Core.Constants;
using ETG.Core.Services;
using ETG.Data.Repositories;
using ETG.Module.Models;

namespace ETG.Data.Currency
{
    public class CurrencyService : ICurrencyService 
    {
        private readonly ICurrencyConversionRetriever _retriever;
        private readonly IFexRepository _fexRepository;
        private readonly ILogger _logger;
        public CurrencyService(ICurrencyConversionRetriever retriever, IFexRepository fexRepository, ILogger logger)
        {
            _retriever = retriever;
            _fexRepository = fexRepository;
            _logger = logger;
        }

        public int Convert(string sourceCurrency, string targetCurrency, int value)
        {
            var exchange = _fexRepository.GetExchangeItem(sourceCurrency, targetCurrency);

            if (exchange == null)
            {
                return 0;
            }

            return (int)Math.Truncate(value / exchange.Amount) ;
        }

        public double GetAdjustedExchangeRate(string sourceCurrency, string targetCurrency)
        {
            var exchange = _fexRepository.GetExchangeItem(sourceCurrency, targetCurrency);

            if (exchange == null || exchange.Amount==0)
            {
                return 0;
            }

            return AdjustConversion(sourceCurrency, exchange.Amount);
        }

        public void GetAndSaveCurrency(string sourceCurrency, string targetCurrency)
        {
            FixerResult result = null;

            try
            {
                result = _retriever.GetLatestCurrencyConverion(sourceCurrency, targetCurrency);

            }
            catch (Exception ex)
            {
                _logger.LogException("CONVERSION", "GET", ex, ex.Message);
                throw ex;
            }

            if (result == null || result.rates == null)
            {
                var ex = new Exception("Currency retriver failed.");
                _logger.LogException("CONVERSION", "NULL", ex, string.Empty);
                throw ex;
            }

           
            SaveConversion(sourceCurrency, CurrencyConstants.CODE_EUR, result.rates.EUR);
            SaveConversion(sourceCurrency, CurrencyConstants.CODE_USD, result.rates.USD);
        }

        private void SaveConversion(string sourceCurrency, string targetCurrency, double rate)
        {
            if (rate == 0)
            {

                _logger.LogInformation("CONVERSION", "NotSaved", $"SOURCE={sourceCurrency}, TARGET={targetCurrency}");

                return;
            }
            var adjustedRate = AdjustConversion(sourceCurrency, rate);
            _fexRepository.Save(sourceCurrency, targetCurrency, rate, adjustedRate);
            _logger.LogInformation("CONVERSION", "SAVE", $"SOURCE={sourceCurrency}, TARGET={targetCurrency}, RATEFROMAPI={rate}, , Adjusted={adjustedRate}");


        }

        private double AdjustConversion(string sourceCurrency, double rate)
        { 
            return  (Math.Truncate(rate * 100) - 1) /100;
         
        }
    }
}

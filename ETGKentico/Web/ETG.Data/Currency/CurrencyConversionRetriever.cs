using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using ETG.Module.Models;
using Newtonsoft.Json;

namespace ETG.Data.Currency
{
    public class CurrencyConversionRetriever : ICurrencyConversionRetriever
    {
        private readonly string _accessKey;
        private readonly string _endpoint;
        public CurrencyConversionRetriever()
        {
            _accessKey = ConfigurationManager.AppSettings["FixerAPIKey"];
            _endpoint = $"http://data.fixer.io/api/latest?access_key={_accessKey}";
        }


        public FixerResult GetLatestCurrencyConverion(string sourceCurrency, string targetCurrency)
        {
            try
            {
                if (string.IsNullOrEmpty(_accessKey))
                {
                    throw new Exception("API key is null");
                }

                var remoteUri = $"{_endpoint}&symbols={targetCurrency}&base={sourceCurrency}";

                WebClient myWebClient = new WebClient();
                // Download the Web resource and save it into a data buffer.
                byte[] myDataBuffer = myWebClient.DownloadData(remoteUri);

                // Display the downloaded data.
                string download = Encoding.ASCII.GetString(myDataBuffer);

                var result = JsonConvert.DeserializeObject<FixerResult>(download);

                if (result != null && result.success)
                {
                    return result;
                }

                var errorResult = JsonConvert.DeserializeObject<FixerErrorResult>(download);

                if (errorResult != null && errorResult.success == false)
                {
                    throw new Exception($"Fixer error: {errorResult.error.type}. Url:{remoteUri}");
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
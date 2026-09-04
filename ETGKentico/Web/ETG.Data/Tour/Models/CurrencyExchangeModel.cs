using System;

namespace ETG.Data.Tour.Models
{
    public class CurrencyExchangeModel
    {
        public double Amount
        {
            get; set;
        }

        public string CurrencySymbol { get; set; }

        public string FormattedAmount
        {
            get { return $"{CurrencySymbol}{Math.Ceiling(Amount).ToString("#,###")}"; }
        }
    }
}

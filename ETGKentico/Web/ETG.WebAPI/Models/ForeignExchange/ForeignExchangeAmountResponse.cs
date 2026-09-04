using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETG.Data.Tour.Models;

namespace ETG.WebAPI.Models.ForeignExchange
{
    public class ForeignExchangeAmountResponse : BaseResponse
    {
        public CurrencyExchangeModel CurrencyExchange { get; set; }
    }
}
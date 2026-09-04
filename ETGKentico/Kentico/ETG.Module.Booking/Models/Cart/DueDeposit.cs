using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Module.Booking.Models.Cart
{
    public class DueDeposit
    {
        public string DueCopy { get; set; }
        public ItemBreakdown DepositPrice { get; set; }
        /*public ItemBreakdown EntireFlexPricing { get; set; }*/

        public double TotalDeposit => (DepositPrice?.TotalPrice ?? 0);// + (EntireFlexPricing?.TotalPrice ?? 0);
        public double TotalDepositeInAUD { get; set; }
    }
}
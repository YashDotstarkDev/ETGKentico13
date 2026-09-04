using System;

namespace ETG.Booking.Pricing.Models
{
    public  class DateBookingPrice
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TwinSharePrice { get; set; }
        public double SingleSupplementalCost { get; set; }
        public double TwinSharePreNightPrice { get; set; }
        public double TwinSharePostNightPrice { get; set; }
        public double SingleSupplementalPreNightCost { get; set; }
        public double SingleSupplementalPostNightCost { get; set; }
        public int DaysToBalanceDueDate { get; set; }
        public int DaysToSecondInstalment { get; set; }
        public double SecondInstalmentPercentage { get; set; }

        public bool HasPrePostNightPrice => TwinSharePreNightPrice > 0 || TwinSharePostNightPrice > 0 ||
                                            SingleSupplementalPreNightCost > 0 || SingleSupplementalPostNightCost > 0;
    }
}

using System;

namespace ETG.Module.Booking.Models.Cart
{
    public class PrePostNightsDetails
    {
        public int NumberOfNights { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public string DateRangeDisplay
        {
            get
            {
                if (DateFrom == DateTime.MinValue || DateTo == DateTime.MinValue)
                {
                    return string.Empty;
                }
                if (DateFrom.Month == DateTo.Month)
                {
                    return $"{DateFrom.Day} - {DateTo.Day} {DateTo:MMM} {DateTo.Year}";
                }

                if (DateFrom.Year != DateTo.Year)
                {
                    return $"{DateFrom:d MMM yyyy} - {DateTo:d MMM yyyy}";
                }
                
                return $"{DateFrom:d MMM} - {DateTo:d MMM yyyy}";
            }
        }
    }
}
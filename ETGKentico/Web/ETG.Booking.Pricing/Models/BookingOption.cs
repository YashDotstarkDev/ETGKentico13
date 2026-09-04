using ETG.Booking.Pricing.Enums;
using System;

namespace ETG.Booking.Pricing.Models
{
    public class BookingOption
    {
        public RoomOptionTypeEnum OptionType { get; set; }
        public string OptionLabel { get; set; }
        public double PricePerPerson { get; set; }        
        public double PreNightPricePerPerson { get; set; }
        public double PostNightPricePerPerson { get; set; }
        public string SoloOptionLabel { get; set; }
        public double SoloPricePerPerson { get; set; }
        public Guid OptionGuid { get; set; }
        
    }
}

using ETG.Booking.Pricing.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Module.Booking.Models.Cart
{
    public class RoomOptionCartItem
    {
        public string OptionDescription { get; set; }
        public double OptionPricePerPerson { get; set; }
        public double PreNightPricePerPerson { get; set; }
        public double PostNightPricePerPerson { get; set; }
        public int OptionType { get; set; }
        public double RoomOptionTotalPrice
        {
            //Different calculation for Type Extras
            get
            {
                if (OptionType == (int)RoomOptionTypeEnum.TwinShareRoomOption)
                {
                    return OptionPricePerPerson * 2;
                }
                
                if (OptionType == (int) RoomOptionTypeEnum.SingleRoomOption)
                {

                    return OptionPricePerPerson;
                }

                return 0;
            }
        }
    }
}

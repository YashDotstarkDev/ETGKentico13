using Castle.Core.Internal;
using System.Collections.Generic;
using System.Linq;

namespace ETG.Module.Booking.Models.Steps
{
    public class BookNowStepRoomOptionsModel : BookNowStepModel
    {
        public string HotelName { get; set; }
        public List<RoomOptionDropdown> RoomOptions { get; set; }

        public bool HasRoomOptions
        {
            get
            {
                if (RoomOptions.IsNullOrEmpty())
                {
                    return false;
                }

                return RoomOptions.Any(a => !a.Options.IsNullOrEmpty());
            }
        }
        
    }
}
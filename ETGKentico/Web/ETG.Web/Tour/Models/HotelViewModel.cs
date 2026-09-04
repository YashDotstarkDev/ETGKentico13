using ETG.Web.Models;
using System;
using System.Collections.Generic;

namespace ETG.Web.Tour.Models
{
    public class HotelViewModel :IViewModel
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Images { get; set; }
        public double Review { get; set; }
        public string Detail { get; set; }
        public bool IsMainHotel { get; set; }
        public string Facilities { get; set; }
        public string Stats { get; set; }

        public List<TourRoomUpgradeViewModel> RoomUpgrades { get; set; }
    }
}
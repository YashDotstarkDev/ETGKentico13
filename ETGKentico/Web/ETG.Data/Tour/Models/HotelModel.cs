using System;
using System.Collections.Generic;
using Devotion.Automapper.Common;

namespace ETG.Data.Tour.Models
{
    public class HotelModel : IDataModel
    {
        public Guid NodeGuid { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; }
        
        public IEnumerable<KeyValuePair<string, string>> Images { get; set; }
        public double Review { get; set; }
        public string Detail { get; set; }
        public bool IsMainHotel { get; set; }
        public string Facilities { get; set; }
        public string Stats { get; set; }
        
        public List<TourRoomUpgradeModel> RoomUpgrades { get; set; }
    }
}
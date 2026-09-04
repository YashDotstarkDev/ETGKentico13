using System;
using Devotion.Automapper.Common;

namespace ETG.Data.Tour.Models
{
    public class TourRoomUpgradeModel : IDataModel
    {
        public string Title { get; set; }
        public string Image { get; set; }

        public string Description
        {
            get; set; 
        }

        public string PriceStatement { get; set; }
        
        public Guid HotelGuid { get; set; }
    }
}

using System;

namespace ETG.Module.Booking.Models
{
    public class RoomOption
    {
        public string Label { get; set; }
        
        public double SupplementalCost { get; set; }
        public Guid OptionGuid { get; set; }
        public string FieldName { get; set; }
        public bool IsSelected { get; set; }
    }
}
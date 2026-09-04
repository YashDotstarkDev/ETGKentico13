using System.Collections.Generic;

namespace ETG.Module.Booking.Models
{
    public class RoomOptionDropdown
    {
        public string FieldName { get; set; }
        public string FieldLabel { get; set; }
        public List<RoomOption> Options { get; set; }
    }
}
namespace ETG.Module.Booking.Models
{
    public class TravellersCountOption
    {
        public int NumberOfRooms { get; set; }
        public int NumberOfPersons { get; set; } 
        public string Label { get; set; }
        public double SupplementalCost { get; set; }
        public bool IsSelected { get; set; }
    }
}
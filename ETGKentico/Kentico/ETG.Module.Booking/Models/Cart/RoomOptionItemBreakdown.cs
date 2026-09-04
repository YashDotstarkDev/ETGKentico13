namespace ETG.Module.Booking.Models.Cart
{
    public class RoomOptionItemBreakdown : ItemBreakdown
    {
        public double PreNightUnitPrice { get; set; }
        public double PostNightUnitPrice { get; set; }

        public double UnitPriceWithPrePostNights { get; set; }

        public double TotalPriceWithPrePostNights => UnitPriceWithPrePostNights * Count;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Module.Booking.Models.Cart
{
    public class ItemBreakdown
    {
        public string ItemLabel { get; set; }
        public string ItemSubLabel { get; set; }
        public DateTime ItemDate { get; set; }
        public double UnitPrice { get; set; }
        public int Count { get; set; }
        public bool HideBreakdown { get; set; }
        public double TotalPrice => UnitPrice * Count;
        public string ItemType { get; set; }
        public int ItemTypeId { get; set; }

        public double GetAdjustedUnitPrice(double totalAdjustmentPrice)
        {
            if (totalAdjustmentPrice == 0)
            {
                return UnitPrice;
            }

            return UnitPrice - Math.Round(totalAdjustmentPrice / Count);
        }
        public double GetAdjustedTotalPrice(double adjustmentPrice)
        {
            return GetAdjustedUnitPrice(adjustmentPrice) * Count;
        } 
    }
}
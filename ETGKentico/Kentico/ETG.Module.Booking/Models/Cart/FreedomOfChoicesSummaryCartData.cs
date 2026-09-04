using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Module.Booking.Models.Cart
{
    public class FreedomOfChoicesSummaryCartData
    {
        public bool SelectNow { get; set; }
        public List<FreedomOfChoiceItem> FreedomOfChoiceItems { get; set; }
    }
}

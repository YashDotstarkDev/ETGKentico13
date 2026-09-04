using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Module.Booking.Models.Cart;

namespace ETG.Module.Booking.ECommerce
{
    public interface IQuoteAddedServiceProvider
    {
        List<ItemBreakdown> GetAddedServices(int quoteId);
    }
}

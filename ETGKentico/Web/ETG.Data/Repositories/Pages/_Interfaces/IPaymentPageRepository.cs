using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Data.Models.Booking;
using ETG.Data.Models.Pages;

namespace ETG.Data.Repositories.Pages
{
    public interface IPaymentPageRepository
    {
        PaymentPageContentsModel GetPaymentPageContents();
    }
}

using CMS.Ecommerce;
using ETG.Web.Models.Pages;

namespace ETG.Web.Models.TourBooking
{
    public class ApplyRefundViewModel
    {
        public OrderCancelPageViewModel PageContents { get; set; }
        public int OrderID { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
    }
}
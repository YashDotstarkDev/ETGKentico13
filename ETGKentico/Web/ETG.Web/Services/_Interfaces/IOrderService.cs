using CMS.Ecommerce;
using ETG.Module.Booking.Classes.Info;

namespace ETG.Web.Services
{
    public interface IOrderService
    {
        void SendConfirmationEmail(OrderInfo order, string refundMemberId);
        void SendNotificationEmail(OrderInfo order);
         void SendConfirmationEmail(PaymentInfo payment);
         void SendNotificationEmail(PaymentInfo payment);
    }
}

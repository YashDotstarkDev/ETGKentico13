using CMS.Ecommerce;
using ETG.Module.Booking.Classes.Info;

namespace ETG.WebAPI.Services
{
    public interface IOrderService
    {
        void SendConfirmationEmail(OrderInfo order, string refundMemberId);
        void SendNotificationEmail(OrderInfo order);
    }
}

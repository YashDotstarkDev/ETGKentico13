using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Ecommerce;
using ETG.Data.Models.Base;
using ETG.Module.Booking.ECommerce.Payment.Models;

namespace ETG.Module.Booking.ECommerce.Payment
{
    public interface IRefundProtectService
    {
        RefundProtectResult PostRefundProtect(OrderInfo order, CustomerInfo customer, bool customerChooseRefundProtect);
        CancelRefundResult CancelOrder(int orderId);
        RefundProtectResult PatchRefundProtectValue(int orderId, double value, bool fromAdmin = false);
        RefundProtectResult PatchRefundProtectDate(int orderId, DateTime newDate, bool fromAdmin = false);
        
        Result SendApplyRefundEmailNotificationToAdmin(OrderInfo order);
        Result SendApplyRefundEmail(OrderInfo order, string refundMemberId);
    }
}

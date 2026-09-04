using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Ecommerce;
using ETG.Module.Booking.Classes.Info;

namespace ETG.Module.Booking.ECommerce.Payment
{
    public interface IRefundProtectRepository
    {
        List<RefundProtectHistoryInfo> GetRefundProtectItems(int orderId);
        RefundProtectHistoryInfo GetRefundProtectItem(int id);
        void AddRefundProtect(OrderInfo order, DateTime summaryDepartureDate);
        void AddRefundProtect(int orderId, double value, DateTime eventDate);
        void LockRefundProtectHistoryItem(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Ecommerce;
using CMS.Helpers;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Classes.Providers;

namespace ETG.Module.Booking.ECommerce.Payment
{
    public class RefundProtectRepository: IRefundProtectRepository
    {
        public List<RefundProtectHistoryInfo> GetRefundProtectItems(int orderId)
        {
            throw new NotImplementedException();
        }

        public RefundProtectHistoryInfo GetRefundProtectItem(int id)
        {
            return RefundProtectHistoryInfoProvider.GetRefundProtectHistory()
                .WhereEquals(nameof(RefundProtectHistoryInfo.RefundProtectHistoryID), id).FirstOrDefault();
        }

        public void AddRefundProtect(OrderInfo order, DateTime summaryDepartureDate)
        {
            var item = new RefundProtectHistoryInfo();
            item.OrderID = order.OrderID;
            item.TransactionDate = DateTime.Now;
            item.Value = ValidationHelper.GetDouble(order.OrderGrandTotalInMainCurrency, 0);
            item.EventDate = summaryDepartureDate;
            item.RefundProtectHistoryLastModified = DateTime.Now;
            item.RefundProtectHistoryGuid = Guid.NewGuid();
            item.IsEditable = true;
            item.Insert();
        }

        public void AddRefundProtect(int orderId, double value, DateTime eventDate)
        {
            var item = new RefundProtectHistoryInfo();
            item.OrderID = orderId;
            item.TransactionDate = DateTime.Now;
            item.Value = value;
            item.EventDate = eventDate;
            item.RefundProtectHistoryLastModified = DateTime.Now;
            item.RefundProtectHistoryGuid = Guid.NewGuid();
            item.IsEditable = true;
            item.Insert();
        }

        public void LockRefundProtectHistoryItem(int id)
        {
            var item = GetRefundProtectItem(id);

            if (item != null)
            {
                item.IsEditable = false;
                item.Update();
            }
        }
    }
}

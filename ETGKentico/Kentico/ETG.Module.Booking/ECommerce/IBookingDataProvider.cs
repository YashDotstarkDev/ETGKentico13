using System;
using CMS.Helpers;
using ETG.Module.Booking.Booking;
using ETG.Module.Booking.Models.Cart;

namespace ETG.Module.Booking.ECommerce
{
    public interface IBookingDataProvider
    {
        CartBookingSummary GetSummaryData(ContainerCustomData customData, bool isOtherPayment = false);
        AddedServicesCartData GetAddedServicesData(ContainerCustomData customData);
    }
}

using CMS.Base.Web.UI;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.UIControls;
using ETG.Module.Booking.Constants;

namespace ETG.Module.Booking.Extenders.Grid
{
    public class RefundProtectGridExtender : ControlExtender<UniGrid>
    {
        private CMSGridActionButton currentActionButton;
        private OrderInfo Order;
        public override void OnInit()
        {

            Control.OnExternalDataBound += Control_OnExternalDataBound;
        }

        private object Control_OnExternalDataBound(object sender, string sourceName, object parameter)
        {
            switch (sourceName)
            {
                case "orderid":
                    if (Order == null)
                    {
                        Order = OrderInfoProvider.GetOrderInfo(ValidationHelper.GetInteger(parameter, 0));
                        
                    }

                    return parameter;
                case "edit":
                    currentActionButton = (CMSGridActionButton) sender;
                    return parameter;
                case "iseditable":
                    var isEditable = ValidationHelper.GetBoolean(parameter, false);

                    if (!isEditable)
                    {
                        currentActionButton.Visible = false;
                    }
                    if (Order.OrderStatusID == BookingConstants.ORDER_STATUS_CANCELLED)
                    {
                        currentActionButton.Visible = false;
                        return "Cancelled";
                    }

                    return "";
                default:
                    return parameter;
              
            }
        }
    }
}

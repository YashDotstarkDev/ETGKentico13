using CMS.Ecommerce;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using System;

namespace CMSApp.CMSFormControls.Custom
{
    public partial class OrderCustomerDetails : FormEngineUserControl
    {
        public CustomerInfo OrderCustomer { get; set; }
        /// <summary>
        /// Gets or sets field value.
        /// </summary>
        public override object Value
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var orderId = QueryHelper.GetInteger("orderid", 0);

            if (orderId <= 0)
            {
                return;
            }

            var order = OrderInfoProvider.GetOrderInfo(orderId);

            if (order?.OrderCustomerID == 0)
            {
                return;
            }

            OrderCustomer = CustomerInfoProvider.GetCustomerInfo(order.OrderCustomerID);
            if (OrderCustomer?.GetStringValue("CustomerAgencyName", string.Empty) != string.Empty)
            {
                plcAgent.Visible = true;
            }
            DataBind();
        }
    }
}
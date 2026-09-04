using Castle.Core.Internal;
using CMS.Base;
using CMS.Ecommerce;
using CMS.Ecommerce.Web.UI;
using CMS.UIControls;
using ETG.Core.Extensions;
using ETG.Data.Extensions;
using ETG.Module.Booking.Classes.Info;
using ETG.Module.Booking.Extensions;
using System;
using CMS.Helpers;
using CMS.Modules;
using ETG.Module.Booking.Classes.Providers;
using ETG.Module.Booking.ECommerce.Payment.Models;
using Newtonsoft.Json;

namespace CMSApp.CMSModules.ETGBooking.PageTemplates
{
    [EditedObject(PaymentInfo.OBJECT_TYPE, "paymentId")]
    public partial class PaymentDetails : CMSDeskPage
    {
     
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            
            this.SetBreadcrumb(0, "", "", "", "");

            var element = UIElementInfoProvider.GetUIElementInfo("ETGBooking", "Payments");

            if (element != null)
            {
                this.SetBreadcrumb(1, "Payments", $"/CMSModules/AdminControls/Pages/UIPage.aspx?elementguid={element.ElementGUID}&displaytitle=false", "", "");
                this.SetBreadcrumb(2, "Payment", "", "", "");
            }
            var payment = PaymentInfoProvider.GetPaymentInfo(QueryHelper.GetInteger("id", 0));
            if (payment != null)
            {
                litMerchatID.Text = payment.PaymentMerchantUniquePaymentId;
                litPaymentCreated.Text = payment.PaymentCreated.ToString("dd/MM/yyyy HH:mm:ss");
                litPaymentId.Text = payment.PaymentID.ToString();
                hidPaymentID.Value = payment.PaymentID.ToString();
                litFirstName.Text = payment.PaymentFirstName;
                litLastName.Text = payment.PaymentLastName;
                litEmail.Text = payment.PaymentEmail;
                litContactNumber.Text = payment.PaymentContactNumber;
                litInvoiceReference.Text = payment.PaymentInvoiceReference;
                litAmount.Text = payment.PaymentAmount.FormatDecimalPrice(true);

                var paymentResult = JsonConvert.DeserializeObject<TravelPayJQueryPaymentResult>(payment.PaymentDetails);

                if (paymentResult != null)
                {
                    litTravelPaymentReference.Text = paymentResult.PaymentReference;
                    litFundsToMerchant.Text = paymentResult.FundsToMerchant.FormatDecimalPrice();
                }


            } 
        }
        
    }
}
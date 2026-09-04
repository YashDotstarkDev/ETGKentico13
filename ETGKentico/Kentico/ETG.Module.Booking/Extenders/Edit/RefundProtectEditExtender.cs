using CMS.Base.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CMS.PortalEngine.Web.UI;
using CMS.UIControls;
using CMS.Base;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Modules;
using CommonServiceLocator;
using ETG.Module.Booking.ECommerce.Payment;
using Newtonsoft.Json;
using System.Web;

namespace ETG.Module.Booking.Extenders.Edit
{
    public class RefundProtectEditExtender : ControlExtender<UIForm>
    {
        private IRefundProtectRepository RefundProtectRepository = ServiceLocator.Current.GetInstance<IRefundProtectRepository>();
        private IRefundProtectService RefundProtectService = ServiceLocator.Current.GetInstance<IRefundProtectService>();
        private int objetId => QueryHelper.GetInteger("objectid", 0);
        private CMSUIPage Page => (CMSUIPage)Control.Page;
        public override void OnInit()
        {
            
            Page.HeaderActions.ActionPerformed += HeaderActions_ActionPerformed;

            var item = RefundProtectRepository.GetRefundProtectItem(objetId);

            if (item != null)
            {
                if (!item.IsEditable)
                {
                    Control.AlternativeFormName = "ViewRefundProtect";
                }
            }
        }
        private string GetValue(string fieldName)
        {
            for (int i = 0; i < Control.Fields.Count; i++)
            {

                if (Control.Fields[i] == fieldName)
                {
                    return ValidationHelper.GetString(Control.FieldControls[Control.Fields[i]].Value, string.Empty);
                }
            }

            return string.Empty;
        }
        private void HeaderActions_ActionPerformed(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            switch (e.CommandName.ToLowerCSafe())
            {
                case "save":
                    var id = Control.EditedObject.GetIntegerValue("RefundProtectHistoryID", 0);
                    var orderId = Control.EditedObject.GetIntegerValue("OrderId", 0);
                    var originalValue = Control.EditedObject.GetDoubleValue("Value", 0);
                    var value = ValidationHelper.GetDouble(GetValue("Value"),0);
                    var originalEventDate = Control.EditedObject.GetDateTimeValue("EventDate", DateTime.MinValue);
                    var eventDate = ValidationHelper.GetDateTime(GetValue("EventDate"), DateTime.MinValue);

                    if (originalValue != value && originalEventDate != eventDate)
                    {
                        Page.ShowError("Cannot change value and date at the same time.");
                        return;
                    }

                    if (originalValue != value)
                    {
                        var result = RefundProtectService.PatchRefundProtectValue(orderId, value - originalValue, true);

                        if (!result.IsSuccessful || result.StatusCode != HttpStatusCode.OK)
                        {
                            Page.ShowError("Refund Protect Patch Value Error.");
                            return;
                        }
                    }

                    if (originalEventDate != eventDate)
                    {
                        var result = RefundProtectService.PatchRefundProtectDate(orderId, eventDate, true);

                        if (!result.IsSuccessful || result.StatusCode != HttpStatusCode.OK)
                        {
                            Page.ShowError("Refund Protect Patch Date Error.");
                            return;
                        }
                    }

                    if (originalValue != value || originalEventDate != eventDate)
                    {
                        using (var t = new CMSTransactionScope())
                        {
                            RefundProtectRepository.LockRefundProtectHistoryItem(id);
                            RefundProtectRepository.AddRefundProtect(orderId, value, eventDate);
                            t.Commit();
                        }
                    }

                    var parentElement = UIElementInfoProvider.GetUIElementInfo("ETGBooking", "RefundProtect");
                    if (parentElement != null)
                    {
                        HttpContext.Current.Response.Redirect($"/CMSModules/AdminControls/Pages/UIPage.aspx?elementguid={parentElement.ElementGUID}&orderid={orderId}&parentobjectid={orderId}&displaytitle=false");
                    }
                    return;
                
                default:

                    return;

            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CMS;
using CMS.Activities;
using CMS.ContactManagement;
using CMS.Helpers;
using CMS.Newsletters;
using CMS.OnlineForms;
using CMS.SiteProvider;
using CommonServiceLocator;
using ETG.Core.Forms;
using ETG.Module.Data;
using ETG.Module.Data.Models;
using ETG.Module.GlobalEvents;
using ETG.Module.Services;

[assembly: RegisterModule(typeof(CustomBizformEvents))]

namespace ETG.Module.GlobalEvents
{
    public class CustomBizformEvents : CMS.DataEngine.Module
    {
        public CustomBizformEvents()
            : base("BizformEvents")
        {
        }

        protected override void OnInit()
        {
            base.OnInit();
            // Assigns custom handlers to events
            BizFormItemEvents.Update.Before += BizFormItem_Update_Before;
            BizFormItemEvents.Insert.After += BizFormItem_Insert_After;
        }

        private void BizFormItem_Update_Before(object sender, BizFormItemEventArgs e)
        {
            if (ConfigurationManager.AppSettings["ImportMode"] != null)
            {
                return;
            }

            if (e.Item.ClassName.ToLower() == EnquireItem.CLASS_NAME.ToLower())
            {
                if (ValidationHelper.GetString(e.Item.GetValue("Status"), string.Empty) == "Confirmed")
                {
                    if (
                        ETGStatusConfirmed.FieldNames.Any(a =>
                            ValidationHelper.GetString(e.Item.GetValue(a), string.Empty) == string.Empty))
                    {
                        e.Cancel();
                        throw new Exception("Booking Reference, Departure date, Return date cannot be null");
                    }
                }
            }
        }

        private void BizFormItem_Insert_After(object sender, BizFormItemEventArgs e)
        {
            if (e.Item.ClassName.ToLower() == CompetitionItem.CLASS_NAME.ToLower() || e.Item.ClassName.ToLower() == CompetitionAgentsItem.CLASS_NAME.ToLower() || e.Item.ClassName.ToLower() == FacebookLeadsItem.CLASS_NAME.ToLower())
            {
                var service = new CompetitionFormContactService();
                service.AddContact(new ContactInfoModel
                {
                    FirstName = ValidationHelper.GetString(e.Item.GetValue("FirstName"), string.Empty),
                    LastName = ValidationHelper.GetString(e.Item.GetValue("LastName"), string.Empty),
                    Email = ValidationHelper.GetString(e.Item.GetValue("EmailAddress"), string.Empty),
                    IsAgent = e.Item.ClassName.ToLower() == CompetitionAgentsItem.CLASS_NAME.ToLower() || ValidationHelper.GetBoolean(e.Item.GetValue("IsTravelAgent"), false)
                }, true);
            }
            else if (e.Item.ClassName.ToLower() == EnquireItem.CLASS_NAME.ToLower())
            {
                var service = new CompetitionFormContactService();
                var contact = service.AddContactForWebsiteConsumer(
                    ValidationHelper.GetString(e.Item.GetValue("FirstName"), string.Empty),
                    ValidationHelper.GetString(e.Item.GetValue("LastName"), string.Empty),
                    ValidationHelper.GetString(e.Item.GetValue("Email"), string.Empty),
                    ValidationHelper.GetString(e.Item.GetValue("Phone"), string.Empty),
                    ValidationHelper.GetString(e.Item.GetValue("PreferredContactMethod"), string.Empty),
                    ValidationHelper.GetBoolean(e.Item.GetValue("SubscribeToNewsletter"), false)
                    );
                if (contact != null)
                {
                    var activity = new ActivityInfo { 
                        ActivityContactID = contact.ContactID,
                        ActivityContactGUID = contact.ContactGUID, 
                        ActivityType = "bizformsubmit", 
                        ActivityCreated = DateTime.Now, 
                        ActivityURLHash = 0,
                        ActivitySiteID = SiteContext.CurrentSiteID,
                        ActivityItemID = 1,
                        ActivityTitle = "Form submitted 'Enquire'",
                        ActivityNodeID = 0,
                        ActivityURLReferrer = string.Empty,

                    };
                    //ActivityInfoProvider.SetActivityInfo(activity);
                    activity.Insert();
                }

            }
        }
    }
}
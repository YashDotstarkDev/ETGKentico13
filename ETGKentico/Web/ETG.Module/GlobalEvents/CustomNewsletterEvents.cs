using CMS;
using CMS.Base;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.Newsletters;
using CMS.SalesForce.WebServiceClient;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using ETG.Module.GlobalEvents;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: RegisterModule(typeof(CustomNewsletterEvents))]
namespace ETG.Module.GlobalEvents
{
    public class CustomNewsletterEvents : CMS.DataEngine.Module
    {
        public CustomNewsletterEvents()
          : base("NewsletterEvents")
        {
        }

        protected override void OnInit()
        {
            base.OnInit();
            // Assigns custom handlers to events
            //NewsletterEvents.SubscriberUnsubscribes.Execute += subscriber_unsubscribes_execute;
            NewsletterInfo.TYPEINFO.Events.Insert.After += newsletter_insert_after;
        }

        private void newsletter_insert_after(object sender, ObjectEventArgs e)
        {
            try
            {
                var nonCampaignNewsletters = NewsletterInfo.Provider.Get()
                    .WhereNotLike(nameof(NewsletterInfo.NewsletterName), "campaign%")
                    .ToList();

                var emails = nonCampaignNewsletters
                    .SelectMany(newsletter => UnsubscriptionInfo.Provider.Get()
                        .WhereEquals(nameof(UnsubscriptionInfo.UnsubscriptionNewsletterID), newsletter.NewsletterID)
                        .Select(x => x.UnsubscriptionEmail))
                    .Distinct()
                    .ToList();

                var newNewsletterId = (e.Object as NewsletterInfo)?.NewsletterID;
                var newNewsletter = NewsletterInfo.Provider.Get().WhereEquals(nameof(NewsletterInfo.NewsletterID), newNewsletterId);
                if (newNewsletter != null)
                {
                    if (newNewsletterId > 0 && emails.Any() && !newNewsletter.FirstOrDefault().NewsletterDisplayName.ToLower().StartsWith("campaign"))
                    {
                        var subscriptionService = CMS.Core.Service.Resolve<ISubscriptionService>();
                        foreach (var email in emails)
                        {
                            if (!subscriptionService.IsUnsubscribed(email, newNewsletterId.Value))
                            {
                                subscriptionService.UnsubscribeFromSingleNewsletter(email, newNewsletterId.Value, null, sendConfirmationEmail: false);
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("NewsletterEvents", "newsletter_insert_after", ex);
            }
        }

        private void subscriber_unsubscribes_execute(object sender, UnsubscriptionEventArgs e)
        {
            try
            {
                var nonCampaignNewsletters = NewsletterInfo.Provider.Get().WhereNotLike(nameof(NewsletterInfo.NewsletterName), "campaign%");

                ISubscriptionService subscriptionService = CMS.Core.Service.Resolve<ISubscriptionService>();
                foreach (var newsletter in nonCampaignNewsletters)
                {
                    if (newsletter != null)
                    {
                        EventLogProvider.LogInformation("NewsletterEvents", $"newsletter id : {newsletter.NewsletterID}");
                        if (!subscriptionService.IsUnsubscribed(e.Email, newsletter.NewsletterID))
                        {
                            if (newsletter.NewsletterID == e.Newsletter.NewsletterID)
                            {
                                EventLogProvider.LogInformation("NewsletterEvents", $"newsletter id : {newsletter.NewsletterID} | newsletter issue id : {e.IssueID}");
                                subscriptionService.UnsubscribeFromSingleNewsletter(e.Email, newsletter.NewsletterID, e.IssueID, sendConfirmationEmail: false);
                            }
                            else
                            {
                                subscriptionService.UnsubscribeFromSingleNewsletter(e.Email, newsletter.NewsletterID, null, sendConfirmationEmail: false);
                            }

                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                EventLogProvider.LogException("NewsletterEvents", "subscriber_unsubscribes_execute", ex);
            }
        }
    }
}
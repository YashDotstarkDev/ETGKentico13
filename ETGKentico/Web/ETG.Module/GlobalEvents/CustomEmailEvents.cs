using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CMS;
using CMS.DataEngine;
using CMS.EmailEngine;
using CMS.Helpers;
using CMS.Newsletters;
using CMS.OnlineForms;
using Devotion.Cache;
using ETG.Core.Forms;
using ETG.Module.Data;
using ETG.Module.GlobalEvents;

[assembly: RegisterModule(typeof(CustomEmailEvents))]
namespace ETG.Module.GlobalEvents
{
    public class CustomEmailEvents : CMS.DataEngine.Module
    {
        public CustomEmailEvents()
            : base("EmailEvents")
        {
        }
        protected override void OnInit()
        {
            base.OnInit();

            // Assigns custom handlers to events
            EmailInfo.TYPEINFO.Events.Insert.Before += Email_Insert_Before;
        }

        private void Email_Insert_Before(object sender, CMS.DataEngine.ObjectEventArgs e)
        {
            ICacheProvider cacheProvider = new KenticoCacheProvider();
            var emailInfo = (EmailInfo)e.Object;

            if (string.IsNullOrEmpty(emailInfo.EmailHeaders))
            {
                return;
            }

            var headers = EmailMessage.GetHeaderFields(emailInfo.EmailHeaders);
            if (headers != null && (headers.AllKeys.Contains("X-CMS-IssueID") ||
                                    (headers.AllKeys.Contains("Importance") && headers["Importance"] != null && headers["Importance"].Trim() == "low")))
            {


                if (headers.AllKeys.Contains("X-CMS-IssueID") && headers["X-CMS-IssueID"] != null)
                {
                    var issueId = ValidationHelper.GetInteger(headers["X-CMS-IssueID"], 0);

                    var issue = cacheProvider.GetCached<IssueInfo>(() => IssueInfoProvider.GetIssueInfo(issueId), $"issue{issueId}", "cms.issue|all", 30);

                    //check transactional automated
                    if (issue != null)
                    {
                        var newsletter = cacheProvider.GetCached<NewsletterInfo>(() =>
                            NewsletterInfoProvider.GetNewsletterInfo(issue.IssueNewsletterID), $"newsletter{issue.IssueNewsletterID}", "newsletter.newsletter|all", 30);
                        if (!string.Equals(newsletter?.NewsletterName, "CampaignEventFeed", StringComparison.OrdinalIgnoreCase))
                        {
                            headers.Add("X-PM-Message-Stream", "default-broadcast-stream");
                        }
                        else
                        {
                            emailInfo.EmailPriority = EmailPriorityEnum.High;
                        }
                    }


                    if (issue != null && !string.IsNullOrEmpty(issue.IssueUTMSource))
                    {
                        headers.Add("X-PM-Tag", issue.IssueUTMSource);
                    }
                }
                else if (headers.AllKeys.Contains("Subject") && headers["Subject"] != null)
                {
                    headers.Add("X-PM-Message-Stream", "default-broadcast-stream");

                    var subject = headers["Subject"].Trim();

                    var issue = cacheProvider.GetCached<IssueInfo>(() => GetSentIssue(subject), $"issue{subject.Replace(" ", "")}", "cms.issue|all", 30);

                    if (issue != null && !string.IsNullOrEmpty(issue.IssueUTMSource))
                    {
                        headers.Add("X-PM-Tag", issue.IssueUTMSource);
                    }
                }

                emailInfo.EmailHeaders = EmailMessage.GetHeaderFields(headers);
            }

        }

        private IssueInfo GetSentIssue(string subject)
        {
            var currentDate = DateTime.Now;

            return IssueInfoProvider.GetIssues().WhereEquals(nameof(IssueInfo.IssueSubject), subject)
                .WhereTrue(nameof(IssueInfo.IssueUseUTM))
                .WhereNotEmpty(nameof(IssueInfo.IssueUTMSource))
                .OrderByDescending(nameof(IssueInfo.IssueLastModified))
                //.WhereNotNull(nameof(IssueInfo.IssueMailoutTime))
                //.WhereNotEquals(nameof(IssueInfo.IssueMailoutTime), DateTime.MinValue)
                //.WhereLessOrEquals(nameof(IssueInfo.IssueMailoutTime), currentDate)
                //.WhereGreaterThan(nameof(IssueInfo.IssueMailoutTime), currentDate.AddHours(-4))
                .FirstOrDefault();
        }
    }
}
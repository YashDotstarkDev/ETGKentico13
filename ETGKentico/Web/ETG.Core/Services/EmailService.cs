using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CMS.DataEngine;
using CMS.EmailEngine;
using CMS.EventLog;
using CMS.MacroEngine;
using CMS.SiteProvider;
using ETG.Core.Kentico;

namespace ETG.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly ISiteContext _siteContext;
        private readonly ILogger _logger;
        public EmailService(ISiteContext siteContext, ILogger logger)
        {
            _siteContext = siteContext;
            _logger = logger;
        }
        public void SendEmail(string templateCode, string to, Dictionary<string, string> replacements, bool isHtml = true, string from = "", string cc = "", string bcc = "", string subject = "")
        {
            var template = EmailTemplateProvider.GetEmailTemplate(templateCode, _siteContext.SiteName);
            var emailSubject = subject;
            if (emailSubject.IsNullOrEmpty())
            {
                EmailHelper.GetSubject(template, "No subject");
            }
            if (template == null)
            {
                _logger.LogInformation( "SendEmail", "TemplateNotExist", "TemplateCode=" + templateCode);
                return;
            }

            var format = EmailFormatEnum.Html;

            if (!isHtml)
            {
                format = EmailFormatEnum.PlainText;
            }
            var email = new EmailMessage
            {
                EmailFormat = format,
                From = string.IsNullOrEmpty(@from) ? EmailHelper.GetSender(template, SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSNoreplyEmailAddress")) : @from,
                Recipients = to,
                Subject = MacroResolver.Resolve(emailSubject),

                CcRecipients = string.IsNullOrEmpty(cc) ? template.TemplateCc : cc,
                BccRecipients = template.TemplateBcc
            };
            MacroResolver resolver = MacroContext.CurrentResolver;

            if (replacements != null)
            {
                foreach (var parameter in replacements)
                {
                    resolver.SetNamedSourceData(parameter.Key, parameter.Value);
                }
            }


            try
            {
                
                EmailSender.SendEmail(SiteContext.CurrentSiteName, email, template.TemplateName, resolver, false);

            }
            catch (Exception ex)
            {
                _logger.LogException( "SendEmail", "Error", ex, string.Empty);
            }

        }
    }
}

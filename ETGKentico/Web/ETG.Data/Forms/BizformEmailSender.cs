using CMS.Base;
using CMS.DataEngine;
using CMS.EmailEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.SiteProvider;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using CMS.OnlineForms;

namespace ETG.Data.Forms
{
    public class BizformEmailSender : IBizformEmailSender
    {
        private static Regex mRegExEmailMacro;
        private string mSiteName;
        private CultureInfo mCulture;
        private MacroResolver mResolver;

        /// <summary>
        /// Regular expression for macros in e-mail body, macros are in form $$type:fieldname$$ where type is "label" or "value".
        /// </summary>
        private static Regex RegExEmailMacro
        {
            get
            {
                return mRegExEmailMacro ?? (mRegExEmailMacro = RegexHelper.GetRegex("\\$\\$\\w+:(?:[A-Za-z]|_[A-Za-z])\\w*\\$\\$"));
            }
        }

        /// <summary>Configuration of the form.</summary>
        protected BizFormInfo FormConfiguration { get; set; }

        /// <summary>Data collected from the form.</summary>
        public IDataContainer FormData { get; set; }

        /// <summary>Form structure definition.</summary>
        protected FormInfo FormDefinition { get; set; }

        /// <summary>Indicates if email content should be encoded.</summary>
        protected bool EncodeEmails { get; set; }

        private string SiteName
        {
            get
            {
                if (this.mSiteName == null && this.FormConfiguration != null)
                    this.mSiteName = SiteInfoProvider.GetSiteName(this.FormConfiguration.FormSiteID);
                return this.mSiteName;
            }
        }

        private CultureInfo Culture
        {
            get
            {
                return this.mCulture ?? (this.mCulture = Thread.CurrentThread.CurrentCulture);
            }
        }

        private MacroResolver Resolver
        {
            get
            {
                if (this.mResolver == null)
                {
                    this.mResolver = MacroResolver.GetInstance(true);
                    this.mResolver.Culture = this.Culture.Name;
                    this.mResolver.Settings.EncodeResolvedValues = this.EncodeEmails;
                    if (this.FormData != null)
                        this.mResolver.SetAnonymousSourceData((object)this.FormData);
                }
                return this.mResolver;
            }
        }

     
        /// <summary>
        /// Sends notification email to specified person based on on-line form configuration and collected data.
        /// </summary>
        public void SendNotificationEmail(BizFormItem bizformItem, string email)
        {
            this.FormConfiguration = bizformItem.BizFormInfo;
            FormData = bizformItem;
            FormDefinition = FormConfiguration?.Form;
            if (this.FormData == null || this.FormConfiguration == null || this.FormDefinition == null)
                return;
            string formSendFromEmail = this.FormConfiguration.FormSendFromEmail;
            string formSendToEmail = email;
            if (string.IsNullOrEmpty(formSendFromEmail) || string.IsNullOrEmpty(formSendToEmail))
                return;
            string str1 = this.Resolver.ResolveMacros(formSendFromEmail, (MacroSettings)null);
            string str2 = this.Resolver.ResolveMacros(formSendToEmail, (MacroSettings)null);
            if (!ValidationHelper.AreEmails((object)str2, (string)null, false))
                throw new ArgumentException("At least one of the specified recipient's email addresses is not valid.");
            string str3 = ResHelper.GetString("BizForm.MessageSubject", (string)null, true) + " - " + this.FormConfiguration.FormDisplayName;
            if (!DataHelper.IsEmpty((object)this.FormConfiguration.FormEmailSubject))
                str3 = this.Resolver.ResolveMacros(this.FormConfiguration.FormEmailSubject, (MacroSettings)null);
            string emailLayout = this.FormConfiguration.FormEmailTemplate;
            if (string.IsNullOrEmpty(emailLayout))
                emailLayout = this.CreateTemplateWithDefaultLayout(this.FormDefinition.GetColumnNames(true, (Func<FormFieldInfo, bool>)null).Where<string>((Func<string, bool>)(col => this.FormData.ContainsColumn(col))));
            string text = this.ResolveEmailMessageText(emailLayout);
            EmailMessage message = new EmailMessage()
            {
                EmailFormat = EmailFormatEnum.Html,
                From = str1,
                Recipients = str2,
                Subject = str3,
                Body = URLHelper.MakeLinksAbsolute(text)
            };
            if (this.EncodeEmails)
                message.Body = HTMLHelper.HTMLEncode(message.Body);
            EmailSender.SendEmail(this.SiteName, message, false);
        }


        /// <summary>Resolve possible field macros in email message text.</summary>
        /// <param name="emailLayout">Email layout template with macros to resolve</param>
        internal string ResolveEmailMessageText(string emailLayout)
        {
            if (string.IsNullOrEmpty(emailLayout))
                return string.Empty;
            emailLayout = this.Resolver.ResolveMacros(emailLayout, (MacroSettings)null);
            MatchCollection matchCollection = RegExEmailMacro.Matches(emailLayout);
            int startIndex = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Match match in matchCollection)
            {
                int index = match.Index;
                if (startIndex < index)
                    stringBuilder.Append(emailLayout.Substring(startIndex, index - startIndex));
                startIndex = index + match.Length;
                string str1 = match.Value.Replace("$$", string.Empty);
                int length = str1.IndexOf(":", StringComparison.Ordinal);
                string str2 = str1.Substring(0, length);
                string str3 = str1.Substring(length + 1, str1.Length - length - 1);
                FormFieldInfo formField = this.FormDefinition.GetFormField(str3);
                if (formField != null)
                {
                    string lowerInvariant = str2.ToLowerInvariant();
                    if (!(lowerInvariant == "label"))
                    {
                        if (lowerInvariant == "value" && this.FormData.ContainsColumn(str3))
                        {
                            string fieldValueForMail = this.GetFieldValueForMail(formField);
                            stringBuilder.Append(fieldValueForMail);
                        }
                    }
                    else
                        stringBuilder.Append(ResHelper.LocalizeString(formField.GetDisplayName((IMacroResolver)this.Resolver), (string)null, false, true));
                }
            }
            if (startIndex < emailLayout.Length)
                stringBuilder.Append(emailLayout.Substring(startIndex, emailLayout.Length - startIndex));
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Returns field value for mail message. Value is transformed according to field type.
        /// </summary>
        /// <param name="formFieldInfo">Form field info</param>
        private string GetFieldValueForMail(FormFieldInfo formFieldInfo)
        {
            object obj = this.FormData.GetValue(formFieldInfo.Name);
            string str = DataTypeManager.GetStringValue(TypeEnum.Field, formFieldInfo.DataType, obj, this.Culture);
            if (formFieldInfo.GetControlName() == "upload" || formFieldInfo.DataType == "bizformfile")
                str = (obj is BizFormUploadFile bizFormUploadFile ? bizFormUploadFile.OriginalFileName : FormHelper.GetOriginalFileName(str));
            return str;
        }

        /// <summary>Returns html code of link to bizform attached file.</summary>
        /// <param name="fileNameString">BizForm file name - guid + extension</param>
        private string GetBizFormAttachmentLink(string fileNameString)
        {
            if (!string.IsNullOrEmpty(fileNameString))
                return string.Format("<a href=\"~/CMSPages/GetBizFormFile.aspx?filename={0}&sitename={1}\">{2}</a>", (object)FormHelper.GetGuidFileName(fileNameString), (object)this.SiteName, (object)FormHelper.GetOriginalFileName(fileNameString));
            return string.Empty;
        }

        /// <summary>
        /// Creates email body template with default layout based on form fields.
        /// </summary>
        /// <param name="columnNames">Form fields</param>
        private string CreateTemplateWithDefaultLayout(IEnumerable<string> columnNames)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string columnName in columnNames)
                stringBuilder.AppendFormat("$$label:{0}$$:      $$value:{0}$$<br /><br />", (object)columnName);
            return stringBuilder.ToString();
        }
    }
}

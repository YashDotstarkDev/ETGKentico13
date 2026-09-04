using System;
using System.Linq;
using Castle.Core.Internal;
using CMS.ContactManagement;
using CMS.CustomTables;
using CMS.Newsletters;
using CMS.SalesForce.WebServiceClient;
using CMS.SiteProvider;
using ETG.Core.CustomTables;
using ETG.Core.Services;
using ETG.Data.Models.Common;
using CMS.Helpers;
using DocumentFormat.OpenXml.Vml.Spreadsheet;

namespace ETG.Data.Services
{
    public class KenticoContactService : IKenticoContactService
    {
        public const string TAG_WEBSITE_TRADE = "Website Trade";
        public const string TAG_WEBSITE_CONSUMER = "Website Consumer";
        private readonly ILogger _logger;
        private readonly IContactProvider _contactProvider;
        private readonly IContactInfoProvider _contactInfoProvider;
        public KenticoContactService(ILogger logger)
        {
            _logger = logger;
            _contactProvider = CMS.Core.Service.Resolve<IContactProvider>();
            _contactInfoProvider = CMS.Core.Service.Resolve<IContactInfoProvider>();
        }

        protected void SetTag(ref string tags, string newTag)
        {
            if (tags.IsNullOrEmpty())
            {
                tags = newTag;
                return;
            }
            if (!tags.Contains(newTag))
            {   
                tags = $"{tags}|{newTag}";
             
            }
        }

        public bool Unsubscribe(string Email, string ReasonId = "")
        {
            if (!string.IsNullOrEmpty(ReasonId))
            {
                var contact = _contactProvider.GetContactForSubscribing(Email);
                var reason = CustomTableItemProvider.GetItem(ValidationHelper.GetInteger(ReasonId, 0), UnsubscribeReasonsItem.CLASS_NAME);
                if (reason != null && contact != null)
                {
                    contact.SetValue("UnsubscribeReason", reason.GetValue("Reason", string.Empty));
                    _contactInfoProvider.Set(contact);
                    return true;
                }
            }
            return false;
        }

        public bool AddContact(string email, bool isAgent)
        {
            var contact = _contactProvider.GetContactForSubscribing(email);
            contact.SetValue("ContactIsAgent", isAgent);

            var tags = contact.GetStringValue("Tags", string.Empty);

            if (isAgent)
            {
                SetTag(ref tags, TAG_WEBSITE_TRADE);

            }
            else
            {
                SetTag(ref tags, TAG_WEBSITE_CONSUMER);
            }

            contact.SetValue("Tags", tags);
            ContactInfoProvider.SetContactInfo(contact);
            return true;
        }
        public bool AddContactForWebsiteConsumer(string firstName, string lastName, string email, string phoneNumber, string preferredContactMethod, bool subscribeToNewsletter)
        {
            var contact =  new ContactInfo();
            if (!string.IsNullOrEmpty(email))
            {
                contact = _contactProvider.GetContactForSubscribing(email);
            }
            //var contact = _contactProvider.GetContactForSubscribing(email);
            contact.ContactFirstName = firstName;
            contact.ContactLastName = lastName;
            contact.ContactMobilePhone = phoneNumber;
            contact.SetValue("ContactPreferredContactMethod", preferredContactMethod);

            if (subscribeToNewsletter)
            {
                var tags = contact.GetStringValue("Tags", string.Empty);

                SetTag(ref tags, TAG_WEBSITE_CONSUMER);
                contact.SetValue("Tags", tags);

            }
            _contactInfoProvider.Set(contact);

            return true;
        }

        public bool ContactHasTags(string email, string[] tags)
        {
            var contact = _contactProvider.GetContactForSubscribing(email);

            if (contact == null || tags.IsNullOrEmpty())
            {
                return false;
            }

            var contactTags = contact.GetValue("Tags", string.Empty);

            if (contactTags.IsNullOrEmpty())
            {
                return false;
            }

            return tags.Any(t => t.Contains(contactTags));
        }

        public bool AddContact(ContactInfoModel contactModel, bool subscribeToNewsletter)
        {
            var contact = _contactProvider.GetContactForSubscribing(contactModel.Email);
            contact.ContactFirstName = contactModel.FirstName;
            contact.ContactLastName = contactModel.LastName;
            if (contact.ContactMobilePhone.IsNullOrEmpty())
            {
                contact.ContactMobilePhone = contactModel.Phone;
            }
            if (subscribeToNewsletter)
            {
                var tags = contact.GetStringValue("Tags", string.Empty);

                if (contactModel.IsAgent)
                {
                    SetTag(ref tags, TAG_WEBSITE_TRADE);
                }
                else
                {
                    SetTag(ref tags, TAG_WEBSITE_CONSUMER);
                }
                contact.SetValue("Tags", tags);

            }
            ContactInfoProvider.SetContactInfo(contact);

            return true;
        }
    }
}

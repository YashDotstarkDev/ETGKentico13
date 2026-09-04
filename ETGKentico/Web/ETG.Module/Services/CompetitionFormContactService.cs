using System;
using System.Linq;
using Castle.Core.Internal;
using CMS.ContactManagement;
using CMS.Newsletters;
using ETG.Core.Services;
using ETG.Module.Data.Models;

namespace ETG.Module.Services
{
    public class CompetitionFormContactService
    {
        public const string TAG_WEBSITE_TRADE = "Website Trade";
        public const string TAG_WEBSITE_CONSUMER = "Website Consumer";
        private readonly IContactProvider _contactProvider;

        public CompetitionFormContactService()
        {
            _contactProvider = CMS.Core.Service.Resolve<IContactProvider>();
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

        public ContactInfo AddContactForWebsiteConsumer(string firstName, string lastName, string email, string phoneNumber,
            string preferredContactMethod, bool subscribeToNewsletter)
        {
            var contact = _contactProvider.GetContactForSubscribing(email);
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

            ContactInfoProvider.SetContactInfo(contact);

            return contact;
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
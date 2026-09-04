using Devotion.Cache;
using ETG.Core.Kentico;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Models.Common;
using ETG.Web.Models.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ETG.Data.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ISiteContext _siteContext;
        private readonly ICacheProvider _cacheProvider;
        public ContactRepository(ISiteContext siteContext, ICacheProvider cacheProvider)
        {
            _siteContext = siteContext;
            _cacheProvider = cacheProvider;
        }

        public ContactModel GetContact(string path)
        {
            return ContactProvider.GetContact(
                    path,
                    _siteContext.CurrentCultureCode,
                    _siteContext.SiteName)
                .Columns(
                    nameof(Contact.ContactOpeningHours),
                    nameof(Contact.ContactAddress1),
                    nameof(Contact.ContactAddress2),
                    nameof(Contact.ContactPhone),
                    nameof(Contact.ContactPhone2),
                    nameof(Contact.HeaderPhoneAu),
                    nameof(Contact.HeaderPhoneNz),
                    nameof(Contact.ContactEmail),
                    nameof(Contact.ContactFacebookUrl),
                    nameof(Contact.ContactLinkedInUrl),
                    nameof(Contact.ContactYouTubeUrl),
                    nameof(Contact.ContactInstagramUrl),
                    nameof(Contact.ContactDisplayAddress),
                    nameof(Contact.ContactDestinationExpertInstruction),
                    nameof(Contact.PeaceOfMindUrl),
                    nameof(Contact.SafeTravelUrl),
                    nameof(Contact.FreedomOfChoiceUrl),
                    nameof(Contact.BookNowUrl),
                    nameof(Contact.FAQsUrl),
                    nameof(Contact.ContactDisplayAddress),
                    nameof(Contact.ContactPreferPhoneDescription),
                    nameof(Contact.ContactPreferPhoneOpeningHoursText),
                    nameof(Contact.ContactPreferPhoneBackgroundImage),
                    nameof(Contact.FreedomOfChoiceDescription),
                    nameof(Contact.SafeTravelDescription),
                    nameof(Contact.ExclusiveProductDescription)
                )
                .Select(a => new ContactModel
                {
                    OpeningHours = a.ContactOpeningHours,
                    Address1 = a.ContactAddress1,
                    Address2 = a.ContactAddress2,
                    HeaderPhoneAu = a.HeaderPhoneAu,
                    HeaderPhoneNz = a.HeaderPhoneNz,
                    PhoneNumber = a.ContactPhone,
                    PhoneNumber2 = a.ContactPhone2,
                    EmailAddress = a.ContactEmail,
                    FacebookUrl = a.ContactFacebookUrl,
                    LinkedInUrl = a.ContactLinkedInUrl,
                    InstagramUrl = a.ContactInstagramUrl,
                    YouTubeUrl = a.ContactYouTubeUrl,
                    DestinationExpertInstruction = a.ContactDestinationExpertInstruction,
                    DisplayAddress = a.ContactDisplayAddress,
                    PreferPhoneDescription = a.ContactPreferPhoneDescription,
                    PreferPhoneOpeningHoursText = a.ContactPreferPhoneOpeningHoursText,
                    PreferPhoneBackgroundImage = a.ContactPreferPhoneBackgroundImage,
                    PeaceOfMindUrl = a.PeaceOfMindUrl,
                    SafeTravelUrl = a.SafeTravelUrl,
                    FreedomOfChoiceUrl = a.FreedomOfChoiceUrl,
                    BookNowUrl = a.BookNowUrl,
                    FAQsUrl = a.FAQsUrl,
                    SafeTravelDescription = a.SafeTravelDescription,
                    ExclusiveProductDescription = a.ExclusiveProductDescription,
                    FreedomOfChoiceDescription = a.FreedomOfChoiceDescription,
                    PeachOfMindCheckList = a.PeaceOfMindCheckList
                })
                .FirstOrDefault();
        }

        public GenericEnquireSidebarModel GenericEnquireSidebar()
        {
            return new GenericEnquireSidebarModel()
            {
                EnquiryContacts = _cacheProvider.GetCached(() => GetEnquiryContactsInternal(),
               "GetEnquiryContacts",
               new GenericDependencyBuilder<EnquiryContact>(_siteContext.SiteName).DependsOnAllNodesOfPageTypeAndOrder())
            };
        }

        private List<EnquiryContactModel> GetEnquiryContactsInternal()
        {
            return EnquiryContactProvider.GetEnquiryContacts().OrderBy("NodeOrder")
               .Select(e => new EnquiryContactModel
               {
                   Name = e.GetStringValue(nameof(EnquiryContact.Name), string.Empty),
                   Phone = e.GetStringValue(nameof(EnquiryContact.Phone), string.Empty),
                   Address = e.GetStringValue(nameof(EnquiryContact.Address), string.Empty),
                   Email = e.GetStringValue(nameof(EnquiryContact.Email), string.Empty),
                   PhoneDigits = Regex.Replace(e.GetStringValue(nameof(EnquiryContact.Phone), string.Empty).Trim(), @"\D", "").Trim() ?? string.Empty,
               })
           .ToList();
        }
    }
}
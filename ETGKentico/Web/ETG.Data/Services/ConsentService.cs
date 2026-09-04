using System.Web;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DataProtection;
using CMS.Helpers;
using ETG.Core.Utility;
using ETG.Data.Configuration;

namespace ETG.Data.Services
{
    public class ConsentService : IConsentService
    {
        private const string ETG_CONSENT_NAME = "ETGCookie";
        private readonly IConsentProvider _consentProvider;
        private readonly IConsentAgreementService _consentAgreementService;
        private readonly IXmlReader _xmlReader;
        public ConsentService(IConsentProvider consentProvider, IXmlReader xmlReader)
        {
            _xmlReader = xmlReader;
            _consentProvider = consentProvider;
            _consentAgreementService = Service.Resolve<IConsentAgreementService>();
        }

        private ConsentInfo GetConsent()
        {
            return _consentProvider.GetConsent(ETG_CONSENT_NAME);
        }

        public void Agree(ContactInfo contact)
        {
            var consent = GetConsent();
            if (consent == null)
            {
                return;
            }

            Service.Resolve<ICurrentCookieLevelProvider>().SetCurrentCookieLevel(CookieLevel.All);

            if (contact != null)
            {
                _consentAgreementService.Agree(contact, consent);
            }
        }

        public void Decline(ContactInfo contact)
        {
            var consent = GetConsent();
            if (consent == null)
            {
                return;
            }

            Service.Resolve<ICurrentCookieLevelProvider>().SetCurrentCookieLevel(CookieLevel.System);

            if (contact != null)
            {
                _consentAgreementService.Revoke(contact, consent);
            }
        }

        public string GetConsentText()
        {
            var consent = GetConsent();
            if (consent?.ConsentContent == null)
            {
                return string.Empty;
            }

            _xmlReader.LoadXml(consent.ConsentContent);

            return HttpUtility.HtmlDecode(
                _xmlReader.GetText("/ConsentContent/ConsentLanguageVersions/ConsentLanguageVersion/ShortText"));
        }
    }
}

using System.Web.Configuration;

namespace ETG.Data.Configuration
{
    public class APIKeyProvider : IApiKeyProvider
    {
        public string RecaptchaSecretKey => WebConfigurationManager.AppSettings["RecaptchaSecretKey"];

        public string RecaptchaSiteKey => WebConfigurationManager.AppSettings["RecaptchaSiteKey"];
        
        public string RecaptchaV3SecretKey => WebConfigurationManager.AppSettings["RecaptchaV3SecretKey"];

         public string RecaptchaV3SiteKey => WebConfigurationManager.AppSettings["RecaptchaV3SiteKey"];
        
        public string RecaptchaV3ScoreThreshold => WebConfigurationManager.AppSettings["CMSRecaptchaV3Threshold"];

        public string GoogleMapAPIKey => WebConfigurationManager.AppSettings["GoogleMapAPIKey"];
    }
}
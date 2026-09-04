using CMS.DataEngine;
using CMS.Helpers;
using Newtonsoft.Json;

namespace ETG.Core.Services.Validation
{
    public class ReCaptchaV3ValidatorService : IReCaptchaV3ValidatorService
    {
        public (bool Success, ReCaptchaV3ValidatorResponse Response) ValidateCaptchaToken(string token, bool isInvisibleRecaptcha)
        {
            var secret = ValidationHelper.GetString(SettingsKeyInfoProvider.GetValue("CMSReCaptchaV3PrivateKey"), string.Empty);
            var threshold = ValidationHelper.GetDecimal(SettingsKeyInfoProvider.GetValue("CMSRecaptchaV3Threshold"), 0.5m);
            
            var client = new System.Net.WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var clientResponse = client.DownloadString($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={token}");
            var result = JsonConvert.DeserializeObject<ReCaptchaV3ValidatorResponse>(clientResponse);

            var success = result.Success && result.Score >= threshold;
            
            return (success, result);
        }
    }
}
using ETG.Data.Configuration;
using Newtonsoft.Json;

namespace ETG.Data.Recaptcha
{
    public class RecaptchaValidator : IRecaptchaValidator
    {
        private readonly IApiKeyProvider _apiKeyProvider;
        public RecaptchaValidator(IApiKeyProvider apiKeyProvider)
        {
            _apiKeyProvider = apiKeyProvider;
        }
        public bool Validate(string encodedResponse)
        {
            if (string.IsNullOrEmpty(encodedResponse)) return false;

            if (string.IsNullOrEmpty(_apiKeyProvider.RecaptchaSecretKey)) return false;

            var client = new System.Net.WebClient();

            var googleReply = client.DownloadString(
                $"https://www.google.com/recaptcha/api/siteverify?secret={_apiKeyProvider.RecaptchaSecretKey}&response={encodedResponse}");

            return JsonConvert.DeserializeObject<RecaptchaResponse>(googleReply).Success;
        }

    }
}

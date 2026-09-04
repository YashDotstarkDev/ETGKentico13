namespace ETG.Core.Services.Validation
{
    public interface IReCaptchaV3ValidatorService
    {
        (bool Success, ReCaptchaV3ValidatorResponse Response) ValidateCaptchaToken(string token, bool isInvisibleRecaptcha);
    }
}
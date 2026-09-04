
namespace ETG.Data.Recaptcha
{
    public interface IRecaptchaValidator
    {
        bool Validate(string responseToken);
    }
}

namespace Devotion.Web.Base.Providers
{
    public interface IAuthenticationProvider<TUserModel>
    {
        TUserModel GetCurrentUser();
    }
}
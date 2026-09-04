using System.Web.Mvc;
using Devotion.Web.Base.Providers;

namespace Devotion.Web.Base.Controllers
{
    public class BaseController<TUserModel> : Controller
    {
        public readonly IAuthenticationProvider<TUserModel> BaseAuthenticationProvider;

        public BaseController(IAuthenticationProvider<TUserModel> baseAuthenticationProvider)
        {
            BaseAuthenticationProvider = baseAuthenticationProvider;
        }

        public TUserModel CurrentUser => BaseAuthenticationProvider.GetCurrentUser();
    }
}
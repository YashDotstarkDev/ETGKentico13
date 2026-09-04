using System.Collections.Generic;
using System.Security.Claims;

namespace Devotion.Web.Base.Providers
{
    public interface IOwinIdentityProvider<TIdentity>
    {
        bool IsLoggedIn { get; }

        List<Claim> Claims { get; }

        bool SignIn(TIdentity identity, bool isPersistent);

        void SignOut();
    }
}
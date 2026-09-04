using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Devotion.Web.Base.Models;

namespace Devotion.Web.Base.Providers
{
    public class BaseOwinIdentityProvider<TIdentity> : IOwinIdentityProvider<TIdentity> where TIdentity : BaseIdentity
    {
        public bool IsLoggedIn => AuthenticationManager.User.Identity.IsAuthenticated;

        public List<Claim> Claims => AuthenticationManager.User.Claims.ToList();

        private static IAuthenticationManager AuthenticationManager =>
            HttpContext.Current.GetOwinContext().Authentication;

        public virtual bool SignIn(TIdentity identity, bool isPersistent)
        {
            if (identity == null)
            {
                throw new ArgumentException("Identity can not be null", nameof(identity));
            }

            IdentitySignIn(MakeIdentityClaims(identity), isPersistent);

            return true;
        }

        public virtual List<Claim> MakeIdentityClaims(TIdentity identity)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, identity.NameIdentifier),
                new Claim(ClaimTypes.Name, identity.Name),
                new Claim(BaseIdentityClaimTypes.USER_ID, identity.UserId.ToString()),
            };
        }

        public virtual void SignOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        private static void IdentitySignIn(List<Claim> claims, bool isPersistent = false)
        {
            AuthenticationManager.SignIn(new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            }, new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie));
        }
    }
}
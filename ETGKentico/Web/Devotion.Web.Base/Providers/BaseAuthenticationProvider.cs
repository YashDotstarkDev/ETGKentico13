using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Devotion.Web.Base.Extensions;
using Devotion.Web.Base.Models;

namespace Devotion.Web.Base.Providers
{
    public class BaseAuthenticationProvider<TUserModel, TIdentity> : IAuthenticationProvider<TUserModel> where TUserModel : BaseUserModel, new()
    {
        private readonly IOwinIdentityProvider<TIdentity> _owinIdentityProvider;

        public BaseAuthenticationProvider(IOwinIdentityProvider<TIdentity> owinIdentityProvider)
        {
            _owinIdentityProvider = owinIdentityProvider;
        }

        public virtual TUserModel GetCurrentUser()
        {
            var userResult = new TUserModel
            {
                IsPublic = true
            };

            var claims = _owinIdentityProvider.Claims;
            if (!claims.Any())
            {
                return userResult;
            }

            MapClaims(userResult, claims);

            return userResult;
        }

        public virtual void MapClaims(TUserModel userResult, List<Claim> claims)
        {
            userResult.Id =
                claims.FirstOrDefault(c => c.Type.Equals(BaseIdentityClaimTypes.USER_ID))?.Value.ToInteger() ?? 0;

            userResult.IsPublic = 
                !_owinIdentityProvider.IsLoggedIn;
        }
    }
}
using Devotion.Web.Base.Providers;
using ETG.Web.Common.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Web.Common.Authentication
{
    public class AuthenticationProvider : BaseAuthenticationProvider<UserModel, Identity>
    {
        public AuthenticationProvider(IOwinIdentityProvider<Identity> owinIdentityProvider)
            : base(owinIdentityProvider)
        { }

    }
}

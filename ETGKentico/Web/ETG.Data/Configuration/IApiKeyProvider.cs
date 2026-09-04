using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Configuration
{
    public interface IApiKeyProvider
    {
        string RecaptchaSecretKey { get;  }
        string RecaptchaSiteKey { get;  }
        string RecaptchaV3SecretKey { get;  }
        string RecaptchaV3SiteKey { get;  }
        string GoogleMapAPIKey { get;  }
    }
}

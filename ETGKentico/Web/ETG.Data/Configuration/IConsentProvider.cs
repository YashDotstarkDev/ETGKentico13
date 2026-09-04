using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.DataProtection;
using ETG.Data.Models.Global;

namespace ETG.Data.Configuration
{
    public interface IConsentProvider
    {
        ConsentInfo GetConsent(string consentName);
    }
}

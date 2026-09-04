using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CMS.DataProtection;
using ETG.Data.Models.Global;

namespace ETG.Data.Configuration
{
    public class ConsentProvider : IConsentProvider
    {
        public ConsentInfo GetConsent(string consentName)
        {
            return ConsentInfoProvider.GetConsentInfo(consentName);

        }
    }
}

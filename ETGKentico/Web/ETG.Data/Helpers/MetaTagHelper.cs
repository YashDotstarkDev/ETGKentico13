using CMS.DataEngine;
using ETG.Data.Settings;

namespace ETG.Data.Helpers
{
    public static class MetaTagHelper
    {

        public static string GetGoogleVerificationCode()
        {
            return SettingsKeyInfoProvider.GetValue(ETGSettingsKey.GoogleVerificationCode);
        }
    }
}

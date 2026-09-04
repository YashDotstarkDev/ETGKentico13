using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;

namespace ETG.Data.Settings
{
    public class SettingsRepository : ISettingsRepository
    {
        private static string[] SettingsKeyNames =>
            new[]
            {
                ETGSettingsKey.FacebookPixelID,
                ETGSettingsKey.GTMID,
                ETGSettingsKey.GoogleAnalyticId,
                ETGSettingsKey.IsLiveEnvironment,
                ETGSettingsKey.BookingRequiredDeposit,
                ETGSettingsKey.EntireFlexThresholdDays,
                ETGSettingsKey.EntireFlexCostPerPerson,
                ETGSettingsKey.BookNowNotificationEmail,
                ETGSettingsKey.ETGPaymentAPIBaseUrl,
                ETGSettingsKey.ETGPaymentAPIKey,
                ETGSettingsKey.ETGPaymentAPIUsername,
                ETGSettingsKey.ETGPaymentAPIPassword,
                ETGSettingsKey.ETGPaymentResultUrl,
                ETGSettingsKey.TravelpayCSSLink,
                ETGSettingsKey.TravelpayJSLink,
                ETGSettingsKey.TravelpayBS5JSLink,
                ETGSettingsKey.RefundProtectEndpoint,
                ETGSettingsKey.RefundProtectVendorID,
                ETGSettingsKey.RefundProtectAPIKey,
                ETGSettingsKey.RefundProtectPremiumRate,
                ETGSettingsKey.RefundProtectMemberID,
                ETGSettingsKey.BookingQuoteValidityDays,
                ETGSettingsKey.ETGPayNowProcessingUrl,
                ETGSettingsKey.SecondInstalmentDays,
                ETGSettingsKey.SecondInstalmentPercentage,
                ETGSettingsKey.SaleComissionPCT,
                ETGSettingsKey.ETGPaymentMerchantCode,
            };

        public Dictionary<string, string> GetETGSettings()
        {
            var settings = SettingsKeyInfoProvider.GetSettingsKeys()
                .WhereIn(nameof(SettingsKeyInfo.KeyName), SettingsKeyNames)
                .WhereNull(nameof(SettingsKeyInfo.SiteID))
                .ToDictionary(a => a.KeyName, a => a.KeyValue);

            var siteSettings = SettingsKeyInfoProvider.GetSettingsKeys()
                .WhereIn(nameof(SettingsKeyInfo.KeyName), SettingsKeyNames)
                .WhereNotNull(nameof(SettingsKeyInfo.SiteID)).WhereGreaterThan(nameof(SettingsKeyInfo.SiteID), 0)
                .ToDictionary(a => a.KeyName, a => a.KeyValue);

            foreach (var siteSetting in siteSettings)
            {
                if (settings.ContainsKey(siteSetting.Key))
                {
                    settings[siteSetting.Key] = siteSetting.Value;
                }
            }

            return settings;
        }
    }
}
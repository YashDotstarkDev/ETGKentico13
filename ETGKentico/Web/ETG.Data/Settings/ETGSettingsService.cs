using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using CMS.DataEngine;
using Devotion.Web.Base.Extensions;
using ETG.Data.Cache;
using ETG.Data.Settings.Models;

namespace ETG.Data.Settings
{
    public class ETGSettingsService : IETGSettingsService
    {
        private readonly Dictionary<string, string> _cachedSettings;
        public ETGSettingsService(ISettingsRepository settingsRepository, ICacheService cacheService)
        {
            _cachedSettings = cacheService.GetAllObjectDependency(
                settingsRepository.GetETGSettings,
                "etgSettings",
                SettingsKeyInfo.OBJECT_TYPE);
        }

        public ETGSettings GetSettings()
        {
            if (_cachedSettings.IsNullOrEmpty())
            {
                return null;
            }

            return new ETGSettings
            {
                FacebookPixelId = _cachedSettings.GetStringValue(ETGSettingsKey.FacebookPixelID),
                GTMID = _cachedSettings.GetStringValue(ETGSettingsKey.GTMID),
                GoogleAnalyticID = _cachedSettings.GetStringValue(ETGSettingsKey.GoogleAnalyticId),
                BookingRequiredDeposit = _cachedSettings.GetDoubleValue(ETGSettingsKey.BookingRequiredDeposit),
                EntireFlexThresholdDays = _cachedSettings.GetIntValue(ETGSettingsKey.EntireFlexThresholdDays),
                EntireFlexCostPerPerson = _cachedSettings.GetDoubleValue(ETGSettingsKey.EntireFlexCostPerPerson),
                ETGPaymentAPIBaseUrl = _cachedSettings.GetStringValue(ETGSettingsKey.ETGPaymentAPIBaseUrl),
                ETGPaymentAPIKey = _cachedSettings.GetStringValue(ETGSettingsKey.ETGPaymentAPIKey),
                ETGPaymentAPIUsername = _cachedSettings.GetStringValue(ETGSettingsKey.ETGPaymentAPIUsername),
                ETGPaymentAPIPassword = _cachedSettings.GetStringValue(ETGSettingsKey.ETGPaymentAPIPassword),
                ETGPaymentResultUrl = _cachedSettings.GetStringValue(ETGSettingsKey.ETGPaymentResultUrl),
                BookNowNotificationEmail = _cachedSettings.GetStringValue(ETGSettingsKey.BookNowNotificationEmail),
                TravelpayCSSLink = _cachedSettings.GetStringValue(ETGSettingsKey.TravelpayCSSLink),
                TravelpayJSLink = _cachedSettings.GetStringValue(ETGSettingsKey.TravelpayJSLink),
                RefundProtectEndpoint = _cachedSettings.GetStringValue(ETGSettingsKey.RefundProtectEndpoint),
                RefundProtectVendorID = _cachedSettings.GetStringValue(ETGSettingsKey.RefundProtectVendorID),
                RefundProtectAPIKey = _cachedSettings.GetStringValue(ETGSettingsKey.RefundProtectAPIKey),
                RefundProtectPremiumRate = _cachedSettings.GetDoubleValue(ETGSettingsKey.RefundProtectPremiumRate),
                RefundProtectMemberID = _cachedSettings.GetStringValue(ETGSettingsKey.RefundProtectMemberID),
                BookingQuoteValidityDays = _cachedSettings.GetIntValue(ETGSettingsKey.BookingQuoteValidityDays),
                ETGPayNowProcessingUrl = _cachedSettings.GetStringValue(ETGSettingsKey.ETGPayNowProcessingUrl),
                SecondInstalmentDays = _cachedSettings.GetIntValue(ETGSettingsKey.SecondInstalmentDays),
                SecondInstalmentPercentage = _cachedSettings.GetDoubleValue(ETGSettingsKey.SecondInstalmentPercentage),
                SaleComissionPCT = _cachedSettings.GetDoubleValue(ETGSettingsKey.SaleComissionPCT),
                TravelpayBS5JSLink = _cachedSettings.GetStringValue(ETGSettingsKey.TravelpayBS5JSLink),
                ETGPaymentMerchantCode = _cachedSettings.GetStringValue(ETGSettingsKey.ETGPaymentMerchantCode),
            };
        }

        public string GetSettingsValue(string keyName)
        {
            return _cachedSettings.Where(a => a.Key == keyName).Select(a => a.Value).FirstOrDefault();
        }
    }
}
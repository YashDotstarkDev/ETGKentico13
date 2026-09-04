using CMS.Core;
using CMS.Helpers;

namespace Devotion.Kentico.Base
{
    public class AbstractKenticoSettingsProvider<T> : ISettingProvider<T>
    {
        private readonly ISettingsService _settingsService;

        protected AbstractKenticoSettingsProvider(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        
        public string GetSetting(T setting)
        {
            return _settingsService[setting.ToString()];
        }

        public TOut GetSetting<TOut>(T setting) where TOut : new()
        {
            return ValidationHelper.GetValue(GetSetting(setting), new TOut());
        }
    }
}
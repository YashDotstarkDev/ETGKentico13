using CMS.Core;
using CMS.Helpers;

namespace Devotion.Kentico.Base
{
    public abstract class AbstractSettingProvider<T> : ISettingProvider<T>
    {
        private readonly IAppSettingsService _appSettingsService;

        protected AbstractSettingProvider(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService;
        }
        
        public string GetSetting(T setting)
        {
            return _appSettingsService[setting.ToString()];
        }
        
        public TOut GetSetting<TOut>(T setting) where TOut : new()
        {
            return ValidationHelper.GetValue(GetSetting(setting), new TOut());
        }
    }
}
namespace Devotion.Kentico.Base
{
    public interface ISettingProvider<in TSetting>
    {
        string GetSetting(TSetting setting);

        TOut GetSetting<TOut>(TSetting setting) where TOut : new();
     }
}
using ETG.Data.Tour.Models;

namespace ETG.Data.Repositories
{
    public interface IFexRepository
    {
        CurrencyExchangeModel GetExchangeItem(string sourceCurrency, string targetCurrency);
        void Save(string sourceCurrency, string targetCurrency, double conversionRate, double adjustedRate);
    }
}

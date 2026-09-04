using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.CustomTables;
using ETG.Core.CustomTables;
using ETG.Data.Tour.Models;

namespace ETG.Data.Repositories
{
    public class FexRepository : IFexRepository
    {
        public CurrencyExchangeModel GetExchangeItem(string sourceCurrency, string targetCurrency)
        {
            var exchangeItem = CustomTableItemProvider.GetItems<FexExchangeItem>()
                .WhereLike(nameof(FexExchangeItem.SourceCurrency), sourceCurrency)
                .WhereLike(nameof(FexExchangeItem.TargetCurrency), targetCurrency)
                .Select(a=> new CurrencyExchangeModel
                {
                    Amount = a.TargetCurrencyValue,
                    
                })
                .FirstOrDefault();

            if (exchangeItem != null)
            {
                return exchangeItem;
            }

            return null;
        }

        public void Save(string sourceCurrency, string targetCurrency,  double conversionRate, double adjustedRate)
        {
            var exchangeItem = CustomTableItemProvider.GetItems<FexExchangeItem>()
                .WhereLike(nameof(FexExchangeItem.SourceCurrency), sourceCurrency)
                .WhereLike(nameof(FexExchangeItem.TargetCurrency), targetCurrency).FirstOrDefault();

            var isNew = false;
            if (exchangeItem == null)
            {
                exchangeItem = new FexExchangeItem
                {
                    SourceCurrency = sourceCurrency,
                    TargetCurrency = targetCurrency,

                };

                isNew = true;
            }

            exchangeItem.TargetCurrencyValue = conversionRate;
            exchangeItem.AdjustedTargetValue = adjustedRate;

            if (isNew)
            {

                exchangeItem.Insert();
            }
            else
            {
                exchangeItem.Update();
            }

        }
    }
}

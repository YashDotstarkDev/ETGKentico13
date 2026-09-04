using CMS.Scheduler;
using CommonServiceLocator;
using ETG.Core.Constants;
using System;
using ETG.Data.Currency;

namespace ETG.Data.Tasks
{
    public class CurrencyConverterTask : ITask
    {
        private ICurrencyService _currencyService;


        public string Execute(TaskInfo task)
        {
            try
            {

                _currencyService = ServiceLocator.Current.GetInstance<ICurrencyService>();
                _currencyService.GetAndSaveCurrency(CurrencyConstants.CODE_AUD, $"{CurrencyConstants.CODE_EUR},{CurrencyConstants.CODE_USD}");
                //_currencyService.GetAndSaveCurrency(CurrencyConstants.CODE_USD, CurrencyConstants.CODE_AUD);

                return $"Last run at {DateTime.Now}. ";
            }
            catch (Exception exception)
            {
                return $"Failed to run the task. The exception is {exception.Message}";
            }
        }
    }
}
using AutoMapper;
using Devotion.Web.Base.Extensions;
using ETG.Data.Repositories;
using ETG.Data.Repositories.Modules;
using ETG.WebAPI.Models;
using ETG.WebAPI.Models.ForeignExchange;
using ETG.WebAPI.Routing;
using System.Threading.Tasks;
using System.Web.Http;


namespace ETG.WebAPI.Controllers
{
    [ApiRoutePrefix("fex")]
    public class ForeignExchangeController : ApiController
    {
        private readonly IFexRepository _fexRepository;
        public ForeignExchangeController(IFexRepository fexRepository)
        {
            _fexRepository = fexRepository;
        }

        [Route("getconvertedamount")]
        public async Task<IHttpActionResult> GetConvertedValue(string sourceCurrency, string targetCurrency, string amount)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };

            var result = _fexRepository.GetExchangeItem(sourceCurrency, targetCurrency);
            result.Amount = result.Amount * amount.ToDouble();
            await Task.FromResult(result);
            if (result == null)
            {
                return Ok(baseResponse);
            }
            var response = new ForeignExchangeAmountResponse
            {
                Success = true,
                CurrencyExchange = result
            };
            return Ok(response);
        }
    }
}
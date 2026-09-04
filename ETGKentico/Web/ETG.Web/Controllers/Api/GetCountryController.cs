using ETG.Data.Repositories;
using System.Threading.Tasks;
using System;
using ETG.Data.Models.Api;
using ETG.Core.Services;
using System.Web.Http;
using ETG.WebAPI.Routing;
using MaxMind.GeoIP2;
using System.Web;
using ETG.Web.Helpers;
using CMS.WebAnalytics;
using System.Linq;

namespace ETG.Web.Controllers.Api { }

[ApiRoutePrefix("getcountry")]
public class GetCountryController : ApiController
{

    private readonly ILogger _logger;
    private readonly ICountryChangeRepository _repository;

    public GetCountryController(ILogger logger, ICountryChangeRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [Route("")]
    [HttpGet]
    public async Task<IHttpActionResult> GetCountry()
    {
        try
        {
            string ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (ipAddress.Contains(","))
            {
                ipAddress = ipAddress.ToString()?.Split(',').First().Trim();
            }

            var arr = ipAddress.ToString()?.Split(':');
            string ip = arr?[0];

            CountryInforModel country = await _repository.GetCountryInfoAsync(ip);
            return Json(country);
        }
        catch (Exception ex)
        {
            _logger.LogException("GetCountryController", "Get", ex, string.Empty);
            return Json(new CountryInforModel { Status = false });
        }
    }
}


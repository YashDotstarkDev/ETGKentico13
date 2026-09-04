using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using ETG.Core.PageTypes;
using ETG.Core.PageTypes.Providers;
using ETG.Module.Booking.HotelBuilder.Models;
using ETG.WebAPI.Routing;

namespace ETG.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [ApiRoutePrefix("hotel")]
    public class HotelBuilderController : ApiController
    {
        private string GetParentName(string nodeAliasPath)
        {
            var index = nodeAliasPath.LastIndexOf("/");

            return nodeAliasPath.Substring(0, index).Replace("-", " ").Replace("/Hotels/", string.Empty).Replace("/", " - ");
        }

        // GET api/<controller>
        [Route("search")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Ok(new HotelSearchResult
            {
                Hotels = HotelProvider.GetHotels().OnCurrentSite().OrderBy(nameof(Hotel.HotelName)).Select(
                    hotel => new HotelItem
                    {
                        ID = hotel.HotelID,
                        Title = hotel.HotelName,
                        Location = GetParentName(hotel.NodeAliasPath)
                    }).ToList()
            });
        }

       
    }
}
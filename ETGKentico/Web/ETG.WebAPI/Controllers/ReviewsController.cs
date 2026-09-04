using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CMS.CustomTables;
using CMS.EventLog;
using ETG.Core.CustomTables;
using ETG.WebAPI.Models;
using ETG.WebAPI.Models.Reviews;
using ETG.WebAPI.Routing;
using Newtonsoft.Json;

namespace ETG.WebAPI.Controllers
{
    [ApiRoutePrefix("reviews")]
    public class ReviewsController : ApiController
    {
        [HttpGet]
        [Route("getreviews")]
        public async Task<IHttpActionResult> Get()
        {
            try
            {

                var googleReviewItem =
                    CustomTableItemProvider.GetItems<GoogleReviewsItem>().FirstOrDefault();

                if (googleReviewItem == null)
                {
                    return BadRequest("Reviews doesnt exist yet");
                }

                var result = JsonConvert.DeserializeObject<ReviewListingResponse>(googleReviewItem.Reviews);
            
                return Ok(result);
            }
            catch (Exception ex)
            {
                EventLogInfo.Provider.Set(new EventLogInfo("E", "GoogleReview", "Err")
                {
                    EventDescription = ex.Message + ex.StackTrace
                });
                return BadRequest(ex.Message);
            }
        }
    }
}
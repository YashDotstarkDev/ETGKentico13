using ETG.WebAPI.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Castle.Core.Internal;
using CMS.EmailEngine;
using CMS.Membership;
using ETG.WebAPI.Models;
using ETG.WebAPI.Models.ForeignExchange;
using ETG.Core.Forms;
using CMS.OnlineForms;
using CMS.SiteProvider;
using Devotion.Web.Base.Extensions;
using ETG.Core.Services;

namespace ETG.WebAPI.Controllers
{
    [ApiRoutePrefix("enquiry")]
    public class EnquiryController : ApiController
    {
        private readonly IEmailService _emailService;
        public EnquiryController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpGet]
        [Route("updatestatus")]
        public async Task<IHttpActionResult> UpdateStatus(string itemId, string status)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };
            var item = BizFormItemProvider.GetItem<EnquireItem>(itemId.ToInteger());

            if (item == null)
            {
                return Ok(baseResponse);
            }

            item.Status = status;
            item.Update();
            baseResponse.Success = true;
            return Ok(baseResponse);
        }

        [HttpGet]
        [Route("assign")]
        public async Task<IHttpActionResult> AssignUser(string itemId, string userguid)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };
            var item = BizFormItemProvider.GetItem<EnquireItem>(itemId.ToInteger());

            if (item == null)
            {
                return Ok(baseResponse);
            }
            var user = UserInfoProvider.GetUsers().WhereEquals(nameof(UserInfo.UserGUID), userguid.ToGuid())
                .FirstOrDefault();

            if (user == null)
            {
                return Ok(baseResponse);
            }
            item.AssignedUserGuid =  userguid.ToGuid();
            item.Update();

            if (!user.Email.IsNullOrEmpty())
            {
                var parameters =  new Dictionary<string,string>();

                parameters.Add("status", item.Status);
                parameters.Add("id", item.ItemID.ToString());
                parameters.Add("firstname", item.Firstname);
                parameters.Add("lastname", item.Lastname);
                parameters.Add("phone", item.Phone);
                parameters.Add("email", item.Email); 
                parameters.Add("destination", item.PreferredDestination);
                parameters.Add("experience", item.PreferredExperience);
                parameters.Add("tourdate", item.TourDate);
                parameters.Add("tourcity", item.TourDepartureCity);
                parameters.Add("tourclass", item.TourClass);
                parameters.Add("tourname", item.TourName);
                _emailService.SendEmail("EnquiryConsultantNotification", user.Email, parameters, false);

            }

            baseResponse.Success = true;
            return Ok(baseResponse);
        }
    }
}

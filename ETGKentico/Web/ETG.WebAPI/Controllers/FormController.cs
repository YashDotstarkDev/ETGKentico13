using AutoMapper;
using ETG.Data.Article.Models;
using ETG.Data.Article.Repositories;
using ETG.WebAPI.Models;
using ETG.WebAPI.Models.ArticleData;
using ETG.WebAPI.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Castle.Core.Internal;
using ETG.Data.Forms;
using ETG.Core.Forms;
using ETG.Data.Models.Common;
using ETG.Data.Services;
using ETG.WebAPI.Models.Forms;
using FluentValidation;

namespace ETG.WebAPI.Controllers
{
    [ApiRoutePrefix("form")]
    public class FormController : ApiController
    {
        private readonly IKenticoContactService _kenticoContactService;
        private readonly AbstractValidator<BrochureSignupItem> _formValidator;
        private readonly AbstractValidator<CompetitionItem> _competitionValidator;
        private readonly IBizformEntry<BrochureSignupItem> _bizFormEntry;
        private readonly CompetitionBizformEntry _bizFormCompetitionEntry;
        private readonly IMapper _mapper;
        public FormController(IMapper mapper, IBizformEntry<BrochureSignupItem> bizFormEntry, CompetitionBizformEntry bizFormCompetitionEntry, AbstractValidator<BrochureSignupItem> formValidator,
            AbstractValidator<CompetitionItem> competitionValidator, IKenticoContactService kenticoContactService)
        {

            _formValidator = formValidator;
            _bizFormEntry = bizFormEntry;
            _mapper = mapper;
            _competitionValidator = competitionValidator;
            _bizFormCompetitionEntry = bizFormCompetitionEntry;
            _kenticoContactService = kenticoContactService;
        }

        [Route("brochuresignup")]
        [HttpPost]
        public async Task<IHttpActionResult> SubscribeToBrochure(Dictionary<string, string> param)
        {
            var baseResponse = new BaseResponse
            {
                Success = false
            };

            if (param == null)
            {
                return Ok(baseResponse);
            }

            var bizformItem = new BrochureSignupItem()
            {
                Name = param["name"],
                Email = param["email"],
                BrochureName = param["brochure"],
                Url = param["url"]

            };


            _bizFormEntry.Initialize(_formValidator, bizformItem);

            if (!_bizFormEntry.Validate())
            {
            }

            var result = _bizFormEntry.Submit();

            await Task.FromResult(result);

            baseResponse.Success = true;
            return Ok(baseResponse);
        }

        [Route("competition")]
        [HttpPost]
        public async Task<IHttpActionResult> JoinCompetition(Dictionary<string, string> param)
        {
            var formResponse = new FormResponse
            {
                Success = false
            };

            try
            {
                if (param == null)
                {
                    return Ok(formResponse);
                }

                var email = param["email"];



                var bizformItem = new CompetitionItem()
                {
                    FirstName = param["firstname"],
                    LastName = param["lastname"],
                    EmailAddress = email,
                    IsTravelAgent = param["isagent"] == "true",
                    HasPreviouslySubscribed = _kenticoContactService.ContactHasTags(email,
                        new[] {KenticoContactService.TAG_WEBSITE_CONSUMER, KenticoContactService.TAG_WEBSITE_TRADE})

                };


                _bizFormCompetitionEntry.Initialize(_competitionValidator, bizformItem);

                if (!_bizFormCompetitionEntry.Validate())
                {
                }

                _kenticoContactService.AddContact(new ContactInfoModel
                {
                    FirstName = bizformItem.FirstName,
                    LastName = bizformItem.LastName,
                    Email = bizformItem.EmailAddress,
                    IsAgent = bizformItem.IsTravelAgent
                }, true);
                var result = _bizFormCompetitionEntry.Submit();

                await Task.FromResult(result);

                formResponse.Success = true;
                var redirectUri = _bizFormCompetitionEntry.GetRedirectUrl();

                if (!redirectUri.IsNullOrEmpty())
                {
                    formResponse.ObjectSet = new FormObjectSet
                    {
                        Actions = new List<FormAction>
                        {
                            new FormAction
                            {
                                Type = "redirect",
                                Uri = redirectUri
                            }
                        }
                    };
                }
            }
            catch(Exception ex)
            {
                formResponse.Success = false;
                formResponse.Message = ex.Message;
            }

            return Ok(formResponse);
        }
    }
}
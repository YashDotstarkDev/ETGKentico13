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
using ETG.Data.Repositories;
using ETG.Data.Services;
using ETG.WebAPI.Models.Forms;
using FluentValidation;

namespace ETG.WebAPI.Controllers
{
    [ApiRoutePrefix("competition")]
    public class CompetitionController : ApiController
    {
        private readonly IKenticoContactService _kenticoContactService;
        private readonly AbstractValidator<CompetitionItem> _competitionValidator;
        private readonly CompetitionBizformEntry _bizFormCompetitionEntry; 
        private readonly AbstractValidator<CompetitionAgentsItem> _competitionAgentValidator;
        private readonly CompetitionAgentBizformEntry _bizFormCompetitionAgentEntry;
        private readonly IMapper _mapper;
        private readonly IHomepageRepository _homepageRepository;
        public CompetitionController(IMapper mapper,
            CompetitionBizformEntry bizFormCompetitionEntry,
            AbstractValidator<CompetitionItem> competitionValidator, 
            CompetitionAgentBizformEntry bizFormCompetitionAgentEntry,
            AbstractValidator<CompetitionAgentsItem> competitionAgentValidator, 
            IKenticoContactService kenticoContactService,
            IHomepageRepository homepageRepository)
        {
            _mapper = mapper;
            _competitionValidator = competitionValidator;
            _competitionAgentValidator = competitionAgentValidator;
            _bizFormCompetitionAgentEntry = bizFormCompetitionAgentEntry;
            _bizFormCompetitionEntry = bizFormCompetitionEntry;
            _kenticoContactService = kenticoContactService;
            _homepageRepository = homepageRepository;
        }
        [Route("popup")]
        [HttpGet]
        public IHttpActionResult PopupOpenSession()
        {
            HttpContext.Current.Session["subscribepopup"] = 1;
            return Ok();
        }

        [Route("subscribe")]
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
                var redirectUri = "";
                if (param["isagent"] != "yes")
                {
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

                    // _kenticoContactService.AddContact(new ContactInfoModel
                    // {
                    //     FirstName = bizformItem.FirstName,
                    //     LastName = bizformItem.LastName,
                    //     Email = bizformItem.EmailAddress,
                    //     IsAgent = bizformItem.IsTravelAgent
                    // }, true);
                    var result = _bizFormCompetitionEntry.Submit();
                    redirectUri = _bizFormCompetitionEntry.GetRedirectUrl();

                    await Task.FromResult(result);
                }
                else
                {
                    var competitionAgentsItem = new CompetitionAgentsItem()
                    {
                        FirstName = param["firstname"],
                        LastName = param["lastname"],
                        EmailAddress = email,
                        HasPreviouslySubscribed = _kenticoContactService.ContactHasTags(email,
                            new[] { KenticoContactService.TAG_WEBSITE_CONSUMER, KenticoContactService.TAG_WEBSITE_TRADE })

                    };


                    _bizFormCompetitionAgentEntry.Initialize(_competitionAgentValidator, competitionAgentsItem);

                    if (!_bizFormCompetitionAgentEntry.Validate())
                    {
                    }

                    // _kenticoContactService.AddContact(new ContactInfoModel
                    // {
                    //     FirstName = competitionAgentsItem.FirstName,
                    //     LastName = competitionAgentsItem.LastName,
                    //     Email = competitionAgentsItem.EmailAddress,
                    //     IsAgent = true
                    // }, true);
                    var result = _bizFormCompetitionAgentEntry.Submit();
                    redirectUri = _bizFormCompetitionAgentEntry.GetRedirectUrl();
                    await Task.FromResult(result);
                }

                formResponse.Success = true;
                formResponse.ObjectSet = new FormObjectSet
                {
                    Actions = new List<FormAction>
                    {
                        new FormAction
                        {
                            Type = "message",
                            Message = _homepageRepository.GetHomepage("/home")?.PopupCompetition?.PopupThankYouMessage
                        }
                    }
                };
                /*if (!redirectUri.IsNullOrEmpty())
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
                }*/
                

                if (param.ContainsKey("cookiename"))
                {
                    var newCookie = new HttpCookie(param["cookiename"], "true");
                    newCookie.Expires = DateTime.Now.AddYears(1);
                    HttpContext.Current.Response.SetCookie(newCookie);
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
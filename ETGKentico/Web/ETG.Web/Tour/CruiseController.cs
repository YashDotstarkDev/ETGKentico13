using System;
using System.Globalization;
using System.Web;
using AutoMapper;
using Devotion.Web.Base.Providers;
using ETG.Data.Tour.Models;
using ETG.Data.Tour.Repositories;
using ETG.Web.Common.Models.Authentication;
using ETG.Web.Tour.Models;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using System.Web.Mvc;
using System.Web.Routing;
using Devotion.Web.Base.Extensions;
using ETG.Core.Forms;
using ETG.Data.Services;
using ETG.PDF;
using ETG.Data.Forms;
using ETG.Web.Helpers;
using ETG.Web.Models.Forms;
using ETG.Web.Models.PageTypes;
using FluentValidation;

namespace ETG.Web.Tour
{
    public class CruiseController : Controller
    {
        private readonly ITourPageRepository _repository;
        private readonly IMapper _mapper;
        private readonly AbstractValidator<EnquireItem> _formValidator;
        private readonly IBizformEntry<EnquireItem> _bizFormEntry;
        private readonly INewsletterService _newsletterService;

        public CruiseController(IMapper mapper,
            ITourPageRepository repository,
            IAuthenticationProvider<UserModel> baseAuthenticationProvider, AbstractValidator<EnquireItem> formValidator,
            IBizformEntry<EnquireItem> bizFormEntry, INewsletterService newsletterService)
        {
            _repository = repository;
            _mapper = mapper;
            _formValidator = formValidator;
            _bizFormEntry = bizFormEntry;
            _newsletterService = newsletterService;
        }

        [HandleError]
        public ActionResult Index(string alias)
        {
            var tour = _repository.Get(string.Empty, alias);
            if (tour == null)
            {
                throw new HttpException(404, "Page not found");
            }
            var tourViewModel = _mapper.Map<TourPageViewModel>(tour);

            return View("~/Views/Tour/Index.cshtml", tourViewModel);

        }
    }
}
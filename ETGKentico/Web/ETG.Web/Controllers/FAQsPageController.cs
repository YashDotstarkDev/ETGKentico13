using System.Web;
using AutoMapper;
using ETG.Core.Forms;
using ETG.Data.Repositories.Forms;
using ETG.Data.Services;
using ETG.Web.Attributes.Filters;
using ETG.Data.Forms;
using ETG.Web.Models.Forms;
using FluentValidation;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using System.Web.Mvc;
using ETG.Core.Constants;
using ETG.Data.Cache;
using ETG.Data.Configuration;
using ETG.Data.Repositories;
using ETG.Data.Repositories._Interfaces;
using ETG.Data.Repositories.Common;
using ETG.Web.Controllers.Base;
using ETG.Web.Helpers;
using ETG.Web.Models.Common;
using ETG.Web.Models.Pages;
using ETG.Web.Models.PageTypes;
using ETG.Web.Services._Interfaces;

namespace ETG.Web.Controllers
{
    public class FAQsPageController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFAQsPageRepository _faqsPageRepository;

        public FAQsPageController(ICacheService cacheService,IMapper mapper, IFAQsPageRepository faqsPageRepository)
        {
            _faqsPageRepository = faqsPageRepository;
            _mapper = mapper;
        }
        
        private FAQsPageViewModel GetViewModel()
        {
            var model = _faqsPageRepository.Get(PathConstants.PATH_FAQsPAGE);
            var viewModel = _mapper.Map<FAQsPageViewModel>(model);
            return viewModel;
        }

        public ActionResult Index()
        {

            var viewModel = GetViewModel();
            ViewBag.HideFaqsButton = true;
            PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.Page.DocumentID);
            return View(viewModel);
        }
    }
}
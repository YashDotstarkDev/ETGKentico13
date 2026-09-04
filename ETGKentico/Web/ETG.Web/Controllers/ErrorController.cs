using System.Net;
using System.Web.Mvc;
using AutoMapper;
using ETG.Data.Repositories.Pages;
using ETG.Web.Models.Pages;

namespace ETG.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IPageNotFoundPageRepository _pageNotFoundPageRepository;
        private readonly IMapper _mapper;
        public ErrorController(IMapper mapper, IPageNotFoundPageRepository pageNotFoundPageRepository)
        {
            _mapper = mapper;
            _pageNotFoundPageRepository = pageNotFoundPageRepository;
        }

        public ActionResult PageNotFound()
        {
            Response.StatusCode = (int) HttpStatusCode.NotFound;
            
            var model = _pageNotFoundPageRepository.GetPageNotFoundPage();
            return model == null 
                ? null 
                : View("PageNotFound", _mapper.Map<PageNotFoundPageViewModel>(model));
        }

        public ActionResult PageError()
        {
            return View("PageError");
        }
    }
}
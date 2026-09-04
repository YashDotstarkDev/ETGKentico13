using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Devotion.Data;
using Devotion.Web.Base.Extensions;
using Devotion.Web.Base.Providers;
using DevTrends.MvcDonutCaching;

namespace Devotion.Web.Base.Controllers
{
    public class PageController<TRepository, TModel, TViewModel, TUserModel> : BaseController<TUserModel>
        where TRepository : IRepository<TModel>
    {
        private readonly IMapper _mapper;
        private readonly TRepository _repository;

        public HttpStatusCode StatusCode = HttpStatusCode.OK;

        public PageController(IMapper mapper, TRepository repository,
            IAuthenticationProvider<TUserModel> baseAuthenticationProvider)
            : base(baseAuthenticationProvider)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HandleError]
        public virtual ActionResult Index()
        {
            var path = HttpContext.Request.Url?.PathAndQuery.GetUrlPathOnly();

            var model = _repository.Get(path, RouteData.GetAlias());
            if (model == null)
            {
                throw new HttpException(404, "Page not found");
            }

            var viewModel = _mapper.Map<TViewModel>(model);

            if (StatusCode != HttpStatusCode.OK)
            {
                Response.StatusCode = (int)StatusCode;
            }

            ProcessBeforeReturningView(viewModel);

            // ReSharper disable once Mvc.ViewNotResolved
            return View(viewModel);
        }

        protected virtual void ProcessBeforeReturningView(TViewModel viewModel)
        {
        }
    }
}
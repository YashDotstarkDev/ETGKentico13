using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Devotion.Data;
using Devotion.Web.Base.Extensions;
using Devotion.Web.Base.Providers;

namespace Devotion.Web.Base.Controllers
{
    public class PageControllerAsync<TRepository, TModel, TViewModel, TUserModel> : BaseController<TUserModel>
        where TRepository : IRepositoryAsync<TModel>
    {
        private readonly IMapper _mapper;
        private readonly TRepository _repository;

        public HttpStatusCode StatusCode = HttpStatusCode.OK;

        public PageControllerAsync(IMapper mapper, TRepository repository,
            IAuthenticationProvider<TUserModel> baseAuthenticationProvider)
            : base(baseAuthenticationProvider)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public virtual async Task<ActionResult> Index()
        {
            var model = await _repository.GetAsync(RouteData.GetAlias());
            var viewModel = _mapper.Map<TViewModel>(model);

            if (StatusCode != HttpStatusCode.OK)
            {
                Response.StatusCode = (int)StatusCode;
            }

            // ReSharper disable once Mvc.ViewNotResolved
            return View(viewModel);
        }
    }
}
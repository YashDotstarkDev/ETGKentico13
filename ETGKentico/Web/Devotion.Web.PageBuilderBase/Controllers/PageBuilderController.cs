
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using Devotion.Data;
using Devotion.Web.Base.Controllers;
using Devotion.Web.Base.Extensions;
using Devotion.Web.Base.Providers;

namespace Devotion.Web.PageBuilderBase.Controllers
{
    public class PageBuilderController<TRepository, TModel, TViewModel, TUserModel> : BaseController<TUserModel>
        where TRepository : IRepository<TModel>
    {
        private readonly IMapper _mapper;
        private readonly TRepository _repository;

        public HttpStatusCode StatusCode = HttpStatusCode.OK;

        public PageBuilderController(IMapper mapper, TRepository repository,
            IAuthenticationProvider<TUserModel> baseAuthenticationProvider)
            : base(baseAuthenticationProvider)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public virtual ActionResult Index()
        {
            var model = _repository.Get(RouteData.GetPath());
            var viewModel = _mapper.Map<TViewModel>(model);

            if (StatusCode != HttpStatusCode.OK)
            {
                Response.StatusCode = (int)StatusCode;
            }

            var documentId = viewModel.GetType().GetProperty("DocumentId")?.GetValue(viewModel, null);
            if (documentId != null)
            {
                HttpContext.Kentico().PageBuilder().Initialize(int.Parse(documentId.ToString()));
            }

            // ReSharper disable once Mvc.ViewNotResolved
            return View(viewModel);
        }
    }
}
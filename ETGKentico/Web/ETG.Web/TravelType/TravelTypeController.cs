using System.Linq;
using AutoMapper;
using Devotion.Web.Base.Providers;
using ETG.Core.Constants;
using ETG.Data.Destination.Models;
using ETG.Data.Destination.Repositories;
using ETG.Web.Common.Models.Authentication;
using ETG.Web.Controllers.Base;
using ETG.Web.Destination.Models;
using ETG.Web.Helpers;
using ETG.Web.Models.Pages;
using System.Web.Mvc;
using Devotion.Web.Base.Extensions;
using ETG.Core.Http;
using ETG.Data.Extensions;
using ETG.Data.Search;
using ETG.Data.TravelType.Models;
using ETG.Data.TravelType.Repositories;
using ETG.Web.Models.Common;
using ETG.Web.Models.Schemas;
using ETG.Web.TravelType.Models;
using Newtonsoft.Json;

namespace ETG.Web.TravelType
{
    public class TravelTypeController : PageController<ITravelTypeDetailPageRepository, TravelTypeDetailPageModel,
        TravelTypeDetailPageViewModel>
    {
        private readonly System.Web.HttpRequest _httpRequest;
        private readonly IMapper _mapper;
        private readonly ITravelTypeDetailPageRepository _repository;
        private readonly ISearchConfiguration _searchConfiguration;

        public TravelTypeController(IMapper mapper,
            ITravelTypeDetailPageRepository repository,
            IAuthenticationProvider<UserModel> baseAuthenticationProvider, ISearchConfiguration searchConfiguration,
            IHttpRequest httpRequest)
            : base(mapper, repository, baseAuthenticationProvider)
        {
            _mapper = mapper;
            _repository = repository;
            _searchConfiguration = searchConfiguration;
            _httpRequest = httpRequest.GetRequest();
        }

        public override ActionResult Index()
        {
            var viewModel =
                _mapper.Map<TravelTypeDetailPageViewModel>(
                    _repository.Get(_httpRequest.Url.PathAndQuery.GetUrlPathOnly()));

            viewModel.ConnectWithUs = new ConnectWithUsViewModel
                { EnquireNowCta = "Enquire about this package", EnquireNowCtaUrl = "/enquire" };

            PageHelper.InitializePageBuilder(HttpContext, viewModel.Detail.DocumentID);
            return View("~/Views/TravelType/Index.cshtml", viewModel);
        }

        public ActionResult Landing()
        {
            var viewModel =
                _mapper.Map<TravelTypeLandingViewModel>(_repository.GetLandingPage(_httpRequest.Url.PathAndQuery.GetUrlPathOnly()));


            PageHelper.InitializePageBuilder(HttpContext, viewModel.DocumentID);
            return View("Landing", viewModel);
        }
    }
}
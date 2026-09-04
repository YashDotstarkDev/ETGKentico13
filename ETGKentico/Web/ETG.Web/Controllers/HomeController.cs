using System.Linq;
using AutoMapper;
using Devotion.Web.Base.Providers;
using ETG.Core.Constants;
using ETG.Data.Extensions;
using ETG.Data.Global;
using ETG.Data.Models.Pages;
using ETG.Data.Repositories.Pages;
using ETG.Data.Search;
using ETG.Web.Common.Models.Authentication;
using ETG.Web.Controllers.Base;
using ETG.Web.Helpers;
using ETG.Web.Models.Pages;
using ETG.Web.Models.Schemas;
using ETG.Web.Services.Menu;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using Newtonsoft.Json;

namespace ETG.Web.Controllers
{
    public class HomeController : PageController<IHomeRepository, HomeModel, HomeViewModel>
    {
        private readonly IMenuService _menuService;
        private readonly ISearchConfiguration _searchConfiguration;
        public HomeController(IMapper mapper,
            IHomeRepository repository,
            IAuthenticationProvider<UserModel> baseAuthenticationProvider,
            IMenuService menuService,
            ISearchConfiguration searchConfiguration)
            : base(mapper, repository, baseAuthenticationProvider)
        {
            _menuService = menuService;
            _searchConfiguration = searchConfiguration;
        }

        protected override void ProcessBeforeReturningView(HomeViewModel viewModel)
        {
            ViewBag.HideFeefo = true;

            viewModel.SearchAppId = _searchConfiguration.ApplicationId;
            viewModel.SearchApiKey = _searchConfiguration.APIKey;
            viewModel.SearchIndexName = _searchConfiguration.IndexTour;
            viewModel.DestinationsMenu = _menuService.GetMenus(PathConstants.PATH_MENU_DESTINATIONS);
            if (viewModel.FAQs!=null)
            {
                var faqSchema = new FAQSchema
                {
                    Context = "https://schema.org",
                    Type = "FAQPage",
                    MainEntity = viewModel?.FAQs.Select(f => new MainEntity
                    {
                        Type = "Question",
                        Name = f.Question,
                        AcceptedAnswer = new AcceptedAnswer { Type = "Answer", Text = f.Answer.StripHtml() }
                    }).ToList()
                };
                viewModel.FAQJsonSchema = JsonConvert.SerializeObject(faqSchema);
            }
            
            if (viewModel?.Page != null)
            {
                PageHelper.InitializePageBuilder(HttpContext, viewModel.Page.DocumentID);
            }
        }
    }
}
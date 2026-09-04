using System.Web.Mvc;
using AutoMapper;
using ETG.Booking.Pricing.Services;
using ETG.Core.Constants;
using ETG.Data.Repositories.Pages;
using ETG.Data.Tour;
using ETG.Web.Models.Pages;
using ETG.Web.Services.Menu;

namespace ETG.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchPageRepository _searchPageRepository; 
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;
        private readonly CurrentCurrencyPricing _currentCurrencyPricing;
        public SearchController(IMapper mapper, ISearchPageRepository searchPageRepository, IMenuService menuService,
            ICurrencyService currencyService)
        {
            _mapper = mapper;
            _searchPageRepository = searchPageRepository;
            _menuService = menuService;
            _currentCurrencyPricing = new CurrentCurrencyPricing(currencyService);

        }

        public ActionResult Index()
        {
            var model = _searchPageRepository.Get();
            if (model == null)
            {
                return null;
            }
            
            return View("Index", _mapper.Map<SearchPageViewModel>(model));
        }
        
        public ActionResult PackageSearch()
        {
            var model = _searchPageRepository.GetPackageSearchPage();
            if (model == null)
            {
                return null;
            }

            var vm = _mapper.Map<PackageSearchPageViewModel>(model);
            
            vm.DestinationMenus =_menuService.GetMenus(PathConstants.PATH_MENU_DESTINATIONS);
            vm.CurrentCurrencyAppliesDiscount = _currentCurrencyPricing.CurrencyAppliesDiscounts;
            return View("PackageSearch", vm);
        }
    }
}
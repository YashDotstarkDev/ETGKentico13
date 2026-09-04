using AutoMapper;
using Castle.Core.Internal;
using ETG.Data.Cache;
using ETG.Data.Repositories.Image;
using ETG.Web.Controllers.Widgets;
using ETG.Web.Models.Common;
using ETG.Web.Models.Widgets.FAQWidget;
using Kentico.PageBuilder.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Devotion.Web.Base.Extensions;
using ETG.Data.Helpers;
using ETG.Data.Repositories.Common;
using ETG.Data.Services;
using ETG.Data.Tour.Models;
using ETG.Data.Tour.Services;
using ETG.Web.Models.Widgets.BrochureCTAWidget;
using ETG.Web.Models.Widgets.ProductsWidget;
using ETG.Web.Tour.Models;

[assembly: RegisterWidget("ETG.Web.Widget.ProductsWidget", typeof(ProductsWidgetController), "Products Widget")]
namespace ETG.Web.Controllers.Widgets
{
    public class ProductsWidgetController : WidgetController<ProductsWidgetProperties>
    {
        private readonly IFAQRepository _faqRepository;
        public readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private IContactService _contactService;

        public ProductsWidgetController(IMapper mapper, IFAQRepository faqRepository, ICacheService cacheService,
            IContactService contactService)
        {
            _faqRepository = faqRepository;
            _cacheService = cacheService;
            _mapper = mapper;
            _contactService = contactService;
        }

        public ActionResult Index()
        {
            var properties = GetProperties();
            
            if (properties.TourCodes.IsNullOrEmpty() && properties.TourType.IsNullOrEmpty() && properties.CruiseType.IsNullOrEmpty())
            {
                return null;
            }
                
            var tourService = DependencyResolver.Current.GetService<ITourService>();
            var mapper = DependencyResolver.Current.GetService<IMapper>();
            List<TourSummaryInfoModel> tours = null;

            if (!properties.TourCodes.IsNullOrEmpty())
            {
                var tourCodeList = properties.TourCodes.Split(',').ToList();
                var unorderedTours = tourService.GetTiledTours(tourCodeList);

                if (!unorderedTours.IsNullOrEmpty())
                {
                    tours = tourCodeList.Select(t => unorderedTours.FirstOrDefault(a => a.TourCode == t)).ToList();
                }
            }
            else if (!properties.TourType.IsNullOrEmpty())
            {
                tours = tourService.GetTiledToursByTourType(properties.TourType, 8);
            }
            else if (!properties.CruiseType.IsNullOrEmpty())
            {
                tours = tourService.GetTiledToursByCruiseType(properties.CruiseType.ToInteger(), 8);
            }
                
            var model = new TourListingViewModel
            {
                HideViewAllButton = properties.HideViewAll,
                ViewAllUrl = properties.ViewAllUrl,
                Tours = mapper.Map<List<TourSummaryInfoViewModel>>(tours)
            };

            var viewModel = new ProductsWidgetViewModel()
            {
                Heading = properties.Heading,
                Tours = model,
                CruiseType = properties.CruiseType,
                TourType = properties.TourType,
                TourCodes = properties.TourCodes,
                SectionId = properties.SectionId,
            };
            return PartialView("Widgets/_ProductsWidget", viewModel);
        }
    }
}
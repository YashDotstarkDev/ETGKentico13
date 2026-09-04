using AutoMapper;
using Castle.Core.Internal;
using ETG.Data.Cache;
using ETG.Web.Controllers.Widgets;
using Kentico.PageBuilder.Web.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;
using ETG.Data.Repositories.Common;
using ETG.Data.Services;
using ETG.Web.Models.Widgets.FeaturedTilesWidget;
using Devotion.Web.Base.Extensions;

[assembly: RegisterWidget("ETG.Web.Widget.FeaturedTiles", typeof(FeaturedTilesWidgetController), "Feature tiles")]

namespace ETG.Web.Controllers.Widgets
{
    public class FeaturedTilesWidgetController : WidgetController<FeaturedTilesWidgetProperties>
    {
        public readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private ICTAImageRepository _cTAImageRepository;

        public FeaturedTilesWidgetController(IMapper mapper, ICacheService cacheService,
            IContactService contactService,
            ICTAImageRepository cTAImageRepository)
        {
            _cacheService = cacheService;
            _mapper = mapper;
            _cTAImageRepository = cTAImageRepository;
        }

        public ActionResult Index()
        {
            var properties = GetProperties();

            if (properties.Path.IsNullOrEmpty())
            {
                return PartialView("Widgets/_FeaturedTiles", new FeaturedTilesWidgetViewModel());
            }

            var path = properties.Path[0].NodeAliasPath;
            var tiles = _cacheService.GetDocumentMultipleDependency(() => _cTAImageRepository.GetImageTileCTAs(path),
                $"featuredtiles_{path}", path);

            var viewModel = new FeaturedTilesWidgetViewModel
            {
                Ctas = _mapper.Map<List<ImageTileCtaViewModel>>(tiles),
                Heading = properties.Heading,
                TileType = properties.TileType.ToInteger(),
                SectionId = properties.SectionId,
            };
            return PartialView("Widgets/_FeaturedTiles", viewModel);
        }
    }
}
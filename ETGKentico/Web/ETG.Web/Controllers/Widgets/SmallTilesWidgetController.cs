using AutoMapper;
using Castle.Core.Internal;
using ETG.Data.Cache;
using ETG.Web.Controllers.Widgets;
using Kentico.PageBuilder.Web.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;
using ETG.Data.Repositories.Common;
using ETG.Data.Services;
using ETG.Web.Models.Widgets.SmallTilesWidget;
using Devotion.Web.Base.Extensions;

[assembly: RegisterWidget("ETG.Web.Widget.SmallTiles", typeof(SmallTilesWidgetController), "Small tiles")]

namespace ETG.Web.Controllers.Widgets
{
    public class SmallTilesWidgetController : WidgetController<SmallTilesWidgetProperties>
    {
        public readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private ICTAImageRepository _cTAImageRepository;

        public SmallTilesWidgetController(IMapper mapper, ICacheService cacheService,
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
                return PartialView("Widgets/_SmallTiles", new SmallTilesWidgetViewModel());
            }

            var path = properties.Path[0].NodeAliasPath;
            var tiles = _cacheService.GetDocumentMultipleDependency(() => _cTAImageRepository.GetImageTileCTAs(path),
                $"SmallTiles_{path}", path);

            var viewModel = new SmallTilesWidgetViewModel
            {
                Ctas = _mapper.Map<List<ImageTileCtaViewModel>>(tiles),
                Heading = properties.Heading,
                SectionId = properties.SectionId,
            };
            return PartialView("Widgets/_SmallTiles", viewModel);
        }
    }
}
using AutoMapper;
using Castle.Core.Internal;
using ETG.Data.Cache;
using ETG.Data.Repositories;
using ETG.Web.Controllers.Widgets;
using ETG.Web.Models.Common;
using Kentico.PageBuilder.Web.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;
using ETG.Data.Repositories.Image;
using ETG.Web.Models.Widgets.ImageSlider;

[assembly: RegisterWidget("ETG.Web.Widget.ImageSlider", typeof(ImageSliderWidgetController), "Image Slider")]
namespace ETG.Web.Controllers.Widgets
{
    public class ImageSliderWidgetController : WidgetController<ImageSliderProperties>
    {
        private readonly IImageRepository _imageRepository;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        public ImageSliderWidgetController(IMapper mapper, IImageRepository imageRepository, ICacheService cacheService)
        {
            _imageRepository = imageRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var properties = GetProperties();

            if (properties.ImagePaths.IsNullOrEmpty())
            {
                return null;
            }
            var path = properties.ImagePaths[0].NodeAliasPath;
            var images = _cacheService.GetDocumentDependentOnChildrenPath(() => _imageRepository.GetImages(path), "imageSlider", path);
            var viewModel = _mapper.Map<List<ImageViewModel>>(images);
            return PartialView("Widgets/_ImageSlider", viewModel);
        }
    }
}
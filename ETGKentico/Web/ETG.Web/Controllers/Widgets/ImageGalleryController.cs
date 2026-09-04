using AutoMapper;
using Castle.Core.Internal;
using ETG.Data.Cache;
using ETG.Data.Repositories;
using ETG.Web.Controllers.Widgets;
using ETG.Web.Models.Common;
using ETG.Web.Models.Widgets.ImageGallery;
using Kentico.PageBuilder.Web.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;
using ETG.Data.Repositories.Image;

[assembly: RegisterWidget("ETG.Web.Widget.ImageGallery", typeof(ImageGalleryController), "Image Gallery")]
namespace ETG.Web.Controllers.Widgets
{
    public class ImageGalleryController : WidgetController<ImageGalleryProperties>
    {
        private readonly IImageRepository _imageRepository;
        public readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        public ImageGalleryController(IMapper mapper, IImageRepository imageRepository, ICacheService cacheService)
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
            var images = _cacheService.GetDocumentDependentOnChildrenPath(() => _imageRepository.GetImages(path), "homeslider", path);
            var viewModel = _mapper.Map<List<ImageViewModel>>(images);
            return PartialView("Widgets/_ImageGallery", viewModel);
        }
    }
}
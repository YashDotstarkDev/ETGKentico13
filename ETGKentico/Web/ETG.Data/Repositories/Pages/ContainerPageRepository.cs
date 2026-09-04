using Castle.Core.Internal;
using ETG.Data.Cache;
using ETG.Data.Models.PageTypes;
using ETG.Data.Repositories.Base;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;

namespace ETG.Data.Repositories.Pages
{
    public class ContainerPageRepository : BasePageRepository, IContainerPageRepository
    {
        private readonly IContainerRepository _containerRepository;
        
        public ContainerPageRepository(IContainerRepository containerRepository,
           ICacheService cacheService, IImageRepository imageRepository, IShareLinksService shareLinksService) : base(imageRepository, cacheService,shareLinksService)
        {
            _containerRepository = containerRepository;
        }
        
        public PageItemModel Get(string url, string path = "")
        {
            var model = CacheService.GetDocumentDependentOnPath
                (() => _containerRepository.GetContainer(url), $"container{url}", url);

            if (model != null)
            {
                if (!string.IsNullOrWhiteSpace(model.RedirectTo))
                {
                    return model;
                }
                
                model.PageHero.GalleryImages = GetGalleryImages(model.Page.PageAliasPath);

                if (!url.IsNullOrEmpty())
                {

                    var arr = url.Split('/');

                    if (arr.Length > 2)
                    {
                        var parentPage = _containerRepository.GetContainer($"/{arr[1]}");
                        if (parentPage != null)
                        {
                            model.BreadCrumbs = GetBreadCrumbs(parentPage.PageName, $"/{arr[1]}", model.PageName);
                        }
                    }
                }

                if (model.BreadCrumbs.IsNullOrEmpty())
                {
                    model.BreadCrumbs = GetBreadCrumbs(model.PageName);
                }
            }

            return model;
        }
    }
}
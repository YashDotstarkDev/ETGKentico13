using System.Collections.Generic;
using Castle.Core.Internal;
using ETG.Data.Cache;
using ETG.Data.Models.Common;
using ETG.Data.Repositories.Image;
using ETG.Data.Services;

namespace ETG.Data.Repositories.Base
{
    public class BasePageRepository
    {
        public readonly IImageRepository ImageRepository;
        public readonly ICacheService CacheService;
        public readonly IShareLinksService ShareLinksService;

        public BasePageRepository(IImageRepository imageRepository, ICacheService cacheService,
            IShareLinksService shareLinksService)
        {
            ImageRepository = imageRepository;
            CacheService = cacheService;
            ShareLinksService = shareLinksService;
        }

        protected virtual ShareLinksModel GetShareLinks(string url, string image = "")
        {
            return new ShareLinksModel
            {
                FacebookShareUrl = ShareLinksService.GetFacebookShareLink(),
                LinkedInShareUrl = ShareLinksService.GetLinkedInShareLink(),
                TwitterShareUrl = ShareLinksService.GetTwitterShareLink(),
                CopyUrl = ShareLinksService.GetCopyLink(),
                PinterestShareUrl = ShareLinksService.GetPinterestShareLink(image)
            };
        }

        protected List<ImageModel> GetGalleryImages(string path)
        {
            return CacheService.GetDocumentDependentOnChildrenPath(
                () => ImageRepository.GetImages($"{path}/Hero-Images"), "GetGalleryImages", path);
        }

        protected List<ImageModel> GetGalleryImagesPlusHeroInternal(string path, ImageModel hero)
        {
            var images = ImageRepository.GetImages(path);

            if (hero != null)
            {
                images.Insert(0, hero);
            }

            return images;
        }

        protected List<ImageModel> GetGalleryImagesPlusHero(string path, ImageModel hero)
        {
            return CacheService.GetDocumentDependentOnChildrenPath(
                () => GetGalleryImagesPlusHeroInternal($"{path}/Hero-Images", hero), "GetGalleryImages", path);
        }

        /// <param name="linkAndUrls">string array: odd param are for labels, even are for url</param>
        protected virtual List<SimpleLinkModel> GetBreadCrumbs(params string[] linkAndUrls)
        {
            if (linkAndUrls.IsNullOrEmpty())
            {
                return null;
            }

            var breadcrumbs = new List<SimpleLinkModel>();

            for (var i = 0; i < linkAndUrls.Length; i++)
            {
                var link = new SimpleLinkModel
                {
                    Label = linkAndUrls[i]
                };

                if (i + 1 < linkAndUrls.Length)
                {
                    link.Url = linkAndUrls[i + 1];
                }

                breadcrumbs.Add(link);
                i++;
            }

            return breadcrumbs;
        }
    }
}
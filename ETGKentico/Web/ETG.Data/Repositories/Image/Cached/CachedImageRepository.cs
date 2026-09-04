using System.Collections.Generic;
using CMS.SiteProvider;
using Devotion.Cache;
using ETG.Data.Models.Common;

namespace ETG.Data.Repositories.Image.Cached
{
    public class CachedImageRepository : ICachedImageRepository
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IImageRepository _imageRepository;

        public CachedImageRepository(ICacheProvider cacheProvider, IImageRepository imageRepository)
        {
            _cacheProvider = cacheProvider;
            _imageRepository = imageRepository;
        }

        public List<ImageModel> GetImages(string path)
        {
            return _cacheProvider.GetCached(() => _imageRepository.GetImages(path),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("getimages")
                    .Append("byaliaspath")
                    .Append(path),
                new GenericDependencyBuilder<Core.PageTypes.Image>(SiteContext.CurrentSiteName)
                    .DependsOnNodeAliasPath(path));
        }
    }
}
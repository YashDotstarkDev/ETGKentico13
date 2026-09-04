using Castle.Core.Internal;
using CMS.Helpers;
using CMS.IO;
using CMS.MediaLibrary;
using ETG.Core.Kentico;
using ETG.Data.Cache;
using ETG.Web.Models.Common;
using System;
using System.Web;

namespace ETG.Web.Services
{
    public class MediaLibraryService : IMediaLibraryService
    {

        private readonly ISiteContext _siteContext;
        private readonly ICacheService _cacheService;

        public MediaLibraryService(ISiteContext siteContext, ICacheService cacheService)
        {
            _siteContext = siteContext;
            _cacheService = cacheService;
        }
        private ImageViewModel GetMediaFileInternal(Guid guid)
        {
            var mediaFile = MediaFileInfoProvider.GetMediaFileInfo(guid, _siteContext.SiteName);
            if (mediaFile != null)
            {
                return new ImageViewModel
                {
                    ImagePath = MediaLibraryHelper.GetPermanentUrl(mediaFile),
                    ImageAltText = mediaFile.FileTitle
                };
            }

            return null;
        }
        public ImageViewModel GetMediaFile(Guid guid)
        {
            return _cacheService.GetMedia(() => GetMediaFileInternal(guid), "media", guid);
        }


    }
}
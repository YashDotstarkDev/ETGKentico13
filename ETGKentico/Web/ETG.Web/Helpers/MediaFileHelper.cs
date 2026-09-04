using ETG.Web.Models.Common;
using ETG.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETG.Web.Helpers
{
    public static class MediaFileHelper
    {
        public static ImageViewModel GetImageMediaFile(Guid guid)
        {
            var mediaLibraryService = DependencyResolver.Current.GetService<IMediaLibraryService>();

            return mediaLibraryService.GetMediaFile(guid);

        }
    }
}
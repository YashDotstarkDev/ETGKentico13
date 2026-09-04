using Castle.Core.Internal;
using CMS.Helpers;
using CMS.IO;
using CMS.MediaLibrary;
using ETG.Core.Kentico;
using ETG.Data.Cache;
using ETG.Web.Models.Common;
using System;
using System.Web;
using CMS.Core;
using CMS.SiteProvider;
using ETG.Core.Utility;

namespace ETG.Web.Services
{
    public class StorageService : IStorageService
    {
        private readonly IClock _clock;

        public StorageService( IClock clock)
        {
            _clock = clock;
        }
        public string SaveFile(string mainFolderPath, HttpPostedFileBase httpPostedFile)
        {
            var fullPath = GetFileFullPath(mainFolderPath, httpPostedFile.FileName);
            StorageHelper.SaveFileToDisk(fullPath, httpPostedFile.InputStream);

            return URLHelper.GetAbsoluteUrl($"/getfile?path={fullPath}", SiteContext.CurrentSite.SitePresentationURL);
        }


        private string GetFileFullPath(string folderPath, string fileName)
        {

            fileName = fileName.Trim('.', ' ');
            var index = fileName.IndexOf(".", StringComparison.Ordinal);
            var extension = string.Empty;
            var fileNameOnly = fileName;
            if (index > -1)
            {
                fileNameOnly = fileName.Substring(0, index) + _clock.Now.ToString("ddMMyyyyHHmmss");
                extension = fileName.Substring(index);
            }

            return $"{folderPath}{fileNameOnly}{extension}";

        }
    }
}
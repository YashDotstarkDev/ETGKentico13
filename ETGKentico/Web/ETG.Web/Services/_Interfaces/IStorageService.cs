using ETG.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.MediaLibrary;

namespace ETG.Web.Services
{
    public interface IStorageService
    {
        string SaveFile(string mainFolderPath, HttpPostedFileBase httpPostedFile);
    }
}
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using CMS.IO;

namespace ETG.Web.Controllers
{
    public class FileController : Controller
    {
        [HandleError]
        public ActionResult Index(string path)
        {
            if (path.IsNullOrEmpty())
            {
                throw new HttpException(404, "Page not found");
            }

            var file = StorageHelper.GetFileInfo(path);

            if (file == null)
            {
                throw new HttpException(404, "Page not found");
            }
            
            Response.AppendHeader("content-disposition", $"inline;filename={file.Name}");
            return new FileStreamResult(StorageHelper.GetFileStream(path, FileMode.Open), "application/octet-stream");
        }
    }
}
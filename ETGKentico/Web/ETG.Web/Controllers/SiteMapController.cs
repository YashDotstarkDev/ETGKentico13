using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CMS.SiteProvider;
using ETG.Data.Models.SiteMap;
using ETG.Data.Repositories;

namespace ETG.Web.Controllers
{
    public class SiteMapController : Controller
    {
        private readonly ISitemapRepository _sitemapRepository;
        public SiteMapController(ISitemapRepository sitemapRepository)
        {
            _sitemapRepository = sitemapRepository;
        }
        private static DateTime GetLocalDateTime(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, DateTimeKind.Local);
        }
        public ActionResult Index()
        {
            Response.ContentType = "text/xml";
            Response.ContentEncoding = Encoding.UTF8;
            Response.BufferOutput = false;

            var items = _sitemapRepository.GetSiteMapItems();
            var baseUrl = SiteContext.CurrentSite.SitePresentationURL;
            var googleSiteMapModel = new GoogleSitemapModel(items.Select(a => new GoogleSitemapNode(a.Url?.ToLower() == "/home" ? baseUrl : baseUrl + a.Url?.ToLower()) { LastModificationDate = a.DateModified }).ToList());

            return (ActionResult)new ETG.Data.Serialization.XmlResult<GoogleSitemapModel>(googleSiteMapModel);

            /*
            return new SitemapProvider().CreateSitemap(new SitemapModel(_sitemapRepository.GetSiteMapItems().Select(
                sM => new SitemapNode(sM.Url)
                {
                    LastModificationDate = GetLocalDateTime(sM.DateModified),
                    
                })
                .ToList()));*/
        }
    }
}
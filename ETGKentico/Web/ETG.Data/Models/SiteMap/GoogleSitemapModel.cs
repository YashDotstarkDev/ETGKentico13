using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ETG.Data.Models.SiteMap
{
    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class GoogleSitemapModel

    {
        public GoogleSitemapModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SimpleMvcSitemap.SitemapModel" /> class.
        /// </summary>
        /// <param name="nodes">Sitemap nodes.</param>
        public GoogleSitemapModel(List<GoogleSitemapNode> nodes)
        {
            this.Nodes = nodes;
        } 
        /// <summary>Sitemap nodes linking to documents</summary>
        [XmlElement("url")]
        public List<GoogleSitemapNode> Nodes { get; }

    }
}

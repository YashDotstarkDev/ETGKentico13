using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ETG.Data.Models.SiteMap
{
    [XmlRoot("url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class GoogleSitemapNode
    {
        public GoogleSitemapNode()
        {

        }
        public GoogleSitemapNode(string url)
        {
            this.Url = url;
        }
        /// <summary>
        /// URL of the page.
        /// This URL must begin with the protocol (such as http) and end with a trailing slash, if your web server requires it.
        /// This value must be less than 2,048 characters.
        /// </summary>
        [XmlElement("loc", Order = 1)]
        public string Url { get; set; }

        /// <summary>
        /// Shows the date the URL was last modified, value is optional.
        /// </summary>
        [XmlElement("lastmod", Order = 2)]
        public string FormattedLastModDate
        {
            get => LastModificationDate.ToString("yyyy-MM-dd");
            set => LastModificationDate = DateTime.Parse(value);
        }

        [XmlIgnore]
        public DateTime LastModificationDate { get; set; }
    }
}

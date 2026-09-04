using ETG.Web.Models.Base;
using ETG.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;

namespace ETG.Web.Article.Models
{
    public class ArticleViewModel : PageNodeViewModel
    {

        public string Title { get; set; }
        public string Summary { get; set; }
        public string HeroImage { get; set; }
        public string HeroImageAltText { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid AuthorGuid { get; set; }
        public string CategoryGuids { get; set; }
        public string CategoriesText { get; set; }
        public string DestinationGuids { get; set; }
        public string AuthorName { get; set; }
        public string DisplayPublishDate { get; set; }
        public string Url { get; set; }

        public KeyValuePair<Guid, string>? Author { get; set; }
        public string DestinationsText
        {
            get
            {
                if (!Destinations.IsNullOrEmpty())
                {
                    return string.Join(", ", Destinations.Select(a => a.Value));
                }

                return string.Empty;
            }
        }
        public List<KeyValuePair<Guid, string>> Destinations { get; set; }
        public List<KeyValuePair<Guid, string>> Categories { get; set; }

        public List<ImageViewModel> GalleryImages { get; set; }
        public string FirstDestinationName { get; set; }
    }
}

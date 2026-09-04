using Castle.Core.Internal;
using ETG.Data.Models.Base;
using ETG.Data.Models.PageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using Devotion.Web.Base.Extensions;
using ETG.Data.Models.Common;

namespace ETG.Data.Article.Models
{
    public class ArticleModel : PageNodeModel
    {
        
        public string Title { get; set; }
        public string Summary { get; set; }
        public string HeroImage { get; set; }
        public string HeroImageAltText { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid AuthorGuid { get; set; }
        public string CategoryGuids { get; set; }
        public string CategoriesText {
            get
            {
                if (!Categories.IsNullOrEmpty())
                {
                    return string.Join(", ", Categories.Select(a=>a.Value));
                }

                return string.Empty;
            }
        }
        public string DestinationGuids { get; set; }

        public Guid FirstDestinationGuid
        {
            get
            {
                if (DestinationGuids.IsNullOrEmpty())
                {
                    return Guid.Empty;
                }

                return DestinationGuids.Split(';')[0].ToGuid();
            }
        }

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
        public string AuthorName
        {
            get
            {
                if (Author == null)
                {
                    return string.Empty;
                }

                return Author.Value.Value;
            }
        }
        public string DisplayPublishDate
        {
            get
            {
                return PublishDate.ToString("dd MMM yyyy");
            }
        }

        public string Url { get; set; }
        public KeyValuePair<Guid, string>? Author { get; set; }

        public List<KeyValuePair<Guid, string>> Destinations { get; set; }
        public List<KeyValuePair<Guid, string>> Categories { get; set; }

        public List<ImageModel> GalleryImages { get; set; }
        public bool DocumentSearchExcluded { get; set; }
        public string FirstDestinationName { get; set; }
    }
}

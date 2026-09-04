using ETG.Data.Article.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Core.Extensions;
using ETG.Web.Article.Models;
using Newtonsoft.Json;

namespace ETG.Web.SEO
{
    public class ArticleJsonSchemaBuilder : IArticleJsonSchemaBuilder
    {
        public string BuildJsonSchema(ArticleViewModel article, string httpDomain)
        {
            if (article == null)
            {
                return string.Empty;
            }

            var rootSchema = new ArticlJSONSchemaRoot
            {
                Context = "https://schema.org",
                Type = "BlogPosting",
                MainEntityOfPage = new MainEntityOfPage
                {
                    Type = "WebPage",
                    Id = $"{httpDomain}{article.Url}"
                },
                Headline = article.Title,
                Description = article.Summary,
                Image = $"{article.HeroImage.Imgixify()}",
                Author = new Author
                {
                    Type = "Organization",
                    Name = "Entire Travel Group"
                },
                Publisher = new Publisher
                {
                    Type = "Organization",
                    Name = "Entire Travel Group Pty Ltd",
                    Logo = new Logo
                    {
                        Type = "ImageObject",
                        Url = "https://www.entiretravel.com.au/icons/apple-touch-icon.png"
                    }
                },
                DatePublished = article.PublishDate.ToString("yyyy-MM-dd"),
                DateModified = article.ModifiedDate.ToString("yyyy-MM-dd")

            };
            
            return JsonConvert.SerializeObject(rootSchema, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        }
    }
}

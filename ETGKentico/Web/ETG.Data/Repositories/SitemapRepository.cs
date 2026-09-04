using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using ETG.Core.Constants;
using ETG.Data.Article.Repositories;
using ETG.Data.Career.Repositories;
using ETG.Data.Destination.Services;
using ETG.Data.Models;
using ETG.Data.Tour.Services;

namespace ETG.Data.Repositories
{
    public class SitemapRepository : ISitemapRepository
    {
 
        private readonly string[] includedClassNames =
            {"ETG.Container", "ETG.Page", "ETG.Destination", "ETG.Experience","ETG.DestinationExpertTeam", "ETG.Homepage","ETG.AgentPortalMainPage", "ETG.TravelTypeLanding"};

        private readonly IArticleRepository _articleRepository;
        private readonly ITourService _tourService;
        private readonly ICareerRoleRepository _careerRoleRepository;
        public SitemapRepository( IArticleRepository articleRepository,
            ITourService tourService, ICareerRoleRepository careerRoleRepository)
        {
            _articleRepository = articleRepository;
            _tourService = tourService;
            _careerRoleRepository = careerRoleRepository;
        }
        
        public List<SiteMapItemModel> GetSiteMapItems()
        {
            var allPages = new List<SiteMapItemModel>();

            allPages.AddRange(GetTreePages());
            allPages.AddRange(GetArticles());
            allPages.AddRange(GetTours());
            allPages.AddRange(GetCareerRoles());
            return allPages;
        }

        private IEnumerable<SiteMapItemModel> GetTreePages()
        {
            const string culture = "en-AU";
            var siteName = SiteContext.CurrentSiteName;

            //Define required parameters for data call
            const string defaultPath = "/%";
            const string defaultWhere = "(DocumentSearchExcluded is NULL OR DocumentSearchExcluded = 0) ";
            const string defaultOrderBy = "NodeLevel, NodeOrder, DocumentName";
            const string defaultColumns = "DocumentModifiedWhen, DocumentUrlPath, NodeAliasPath";

            //Grabbing the list of available types to potentially filter out types like containers and folders
            // but for now, I'm not going to worry about it
            
            Func<MultiDocumentQuery> dataLoadMethod = () => DocumentHelper.GetDocuments()
                .PublishedVersion(true)
                .Types(includedClassNames)
                .Path(defaultPath)
                .Culture(culture)
                .CombineWithDefaultCulture(true)
                .Where(defaultWhere)
                .OrderBy(defaultOrderBy)
                .Published(true)
                .Columns(defaultColumns);

            var cacheSettings = new CacheSettings(240, "data|xmlsitemap", siteName, culture)
            {
                GetCacheDependency = () =>
                {
                    // Creates caches dependencies. This example makes the cache clear data when any node is modified, deleted, or created.
                    var dependencyCacheKey = $"node|{siteName}|/|childnodes";
                    return CacheHelper.GetCacheDependency(dependencyCacheKey);
                }
            };
            
            return CacheHelper.Cache(dataLoadMethod, cacheSettings).Select(a => new SiteMapItemModel
            {
                Url = a.NodeAliasPath,
                DateModified = a.DocumentModifiedWhen
            }).ToList();
        }

        private IEnumerable<SiteMapItemModel> GetCareerRoles()
        {
            var roles = _careerRoleRepository.GetCareerRoles(PathConstants.PATH_CAREER_ROLES);
            if (roles == null)
            {
                return Enumerable.Empty<SiteMapItemModel>();
            }

            return roles
                .Where(x => !x.DocumentSearchExcluded)
                .Select(a => new SiteMapItemModel
            {
                Url = a.Path,
                DateModified = a.DateModified
            });
        }

        private IEnumerable<SiteMapItemModel> GetTours()
        {
            var tours = _tourService.GetTiledTours(1000);
            if (tours == null)
            {
                return Enumerable.Empty<SiteMapItemModel>();
            }

            return tours
                .Where(x => !x.DocumentSearchExcluded)
                .Select(a => new SiteMapItemModel
                {
                    Url = a.Path,
                    DateModified = a.DateModifed
                });
        }

        private IEnumerable<SiteMapItemModel> GetArticles()
        {
            var articles = _articleRepository.GetLatestArticles(1000);
            if (articles == null)
            {
                return Enumerable.Empty<SiteMapItemModel>();
            }

            return articles
                .Where(x => !x.DocumentSearchExcluded)
                .Select(a => new SiteMapItemModel
                {
                    Url = a.Url,
                    DateModified = a.ModifiedDate
                });
        }
    }
}

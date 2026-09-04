using System.Collections.Generic;
using System.Linq;
using CMS.SiteProvider;
using Devotion.Cache;
using ETG.Core.Constants;
using ETG.Core.PageTypes;
using ETG.Data.Models.Common;
using ETG.Data.Repositories.Common;
using ETG.Web.Models.Common;
using ETG.Web.Models.Menu;

namespace ETG.Web.Services.Menu.Cached
{
    public class CachedMenuService : ICachedMenuService
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IMenuService _menuService;
        private readonly ICTAIconRepository _ctaIconRepository;

        public CachedMenuService(ICacheProvider cacheProvider, IMenuService menuService,
            ICTAIconRepository ctaIconRepository)
        {
            _cacheProvider = cacheProvider;
            _menuService = menuService;
            _ctaIconRepository = ctaIconRepository;
        }

        public List<MainMenuViewModel> GetMainMenuViewModel()
        {
            return _cacheProvider.GetCached(() => _menuService.GetMainMenuViewModel(),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("getmainmenuviewmodel")
                    .Append("byaliaspath")
                    .Append(PathConstants.PATH_MAIN_MENU),
                new GenericDependencyBuilder<Link>(SiteContext.CurrentSiteName)
                    .DependsOnNodeAliasPath(PathConstants.PATH_MAIN_MENU));
        }

        public List<MenuGroup> GetFooterTopMenus()
        {
            return _cacheProvider.GetCached(() => _menuService.GetFooterTopMenus(),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("getfootertopmenus")
                    .Append("byaliaspath")
                    .Append(PathConstants.PATH_FOOTER_TOP_MENU),
                new GenericDependencyBuilder<Link>(SiteContext.CurrentSiteName)
                    .DependsOnNodeAliasPath(PathConstants.PATH_FOOTER_TOP_MENU));
        }

        public List<LinkViewModel> GetFooterBottomMenus()
        {
            return _cacheProvider.GetCached(() => _menuService.GetFooterBottomMenus(),
                new CacheKeyBuilder(SiteContext.CurrentSiteName)
                    .Append("getfooterbottommenus")
                    .Append("byaliaspath")
                    .Append(PathConstants.PATH_FOOTER_BOTTOM_MENU),
                new GenericDependencyBuilder<Link>(SiteContext.CurrentSiteName)
                    .DependsOnNodeAliasPath(PathConstants.PATH_FOOTER_BOTTOM_MENU));
        }

        public ProofPointComponentViewModel GetFooterProofPoints()
        {
            var count = 6;
            var cmsProofPoints = _cacheProvider.GetCached(
                () => _ctaIconRepository.Get("", PathConstants.PATH_FOOTER_PROOF_POINTS).Take(count),
                $"proofpoints{count}", PathConstants.PATH_FOOTER_PROOF_POINTS);
            if (cmsProofPoints == null)
            {
                return null;
            }

            var proofPoints = 
            cmsProofPoints.Select(x => new CTAIconViewModel
                { Label = x.Label, Url = x.Url, IconClass = x.IconClass, SvgIcon = x.SvgIcon }).ToList();

            return new ProofPointComponentViewModel
            {
                Title = "When experience matters",
                ProofPoints = proofPoints
            };
        }
    }
}
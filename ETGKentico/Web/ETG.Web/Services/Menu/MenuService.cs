using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Castle.Core.Internal;
using Devotion.Web.Base.Extensions;
using ETG.Core.Constants;
using ETG.Data.Cache;
using ETG.Data.Global;
using ETG.Web.Models.Menu;

namespace ETG.Web.Services.Menu
{
    public class MenuService : IMenuService
    {
        private readonly ILinkRepository _linkRepository;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public MenuService(IMapper mapper, ILinkRepository linkRepository, ICacheService cacheService)
        {
            _linkRepository = linkRepository;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public List<MainMenuViewModel> GetMainMenuViewModel()
        {
            var data = _cacheService.GetDocumentDependentOnChildrenPath(
                () => _linkRepository.GetLinks(PathConstants.PATH_MAIN_MENU), "GetMainMenuViewModel",
                PathConstants.PATH_MAIN_MENU);

            var menus = _mapper.Map<List<LinkViewModel>>(data);
            if (menus.IsNullOrEmpty())
            {
                return null;
            }

            var viewModelList = new List<MainMenuViewModel>();
            var mainNodeLevel = menus.Min(a => a.NodeLevel);

            foreach (var menu in menus)
            {
                if (menu.NodeLevel != mainNodeLevel)
                {
                    break;
                }

                viewModelList.Add(new MainMenuViewModel
                {
                    Link = menu,
                    MenuGroups = GetMenuGroups(menus, menu.NodeAliasPath, menu)
                });
            }

            return viewModelList;
        }
        
        public List<MenuGroup> GetMenus(string path)
        {
            var links = _cacheService.GetDocumentDependentOnChildrenPath(
                () => _linkRepository.GetLinks(path), "GetMenus",
                path);

            var menus = _mapper.Map<List<LinkViewModel>>(links);
            if (menus.IsNullOrEmpty())
            {
                return null;
            }

            var viewModelList = new List<MainMenuViewModel>();
            var mainNodeLevel = menus.Min(a => a.NodeLevel);

            var menuGroups = new List<MenuGroup>();
            
            foreach (var menu in menus)
            {
                if (menu.NodeLevel != mainNodeLevel)
                {
                    break;
                }

                menuGroups = GetMenuGroups(menus, menu.NodeAliasPath, menu);

            }

            return menuGroups;
        }

        public List<MenuGroup> GetFooterTopMenus()
        {
            var menus = _mapper.Map<List<LinkViewModel>>(_cacheService.GetDocumentDependentOnChildrenPath(
                () => _linkRepository.GetLinks(PathConstants.PATH_FOOTER_TOP_MENU), "GetFooterTopMenus",
                PathConstants.PATH_FOOTER_TOP_MENU));

            var menuGroups = new List<MenuGroup>();
            var mainNodeLevel = menus.Min(a => a.NodeLevel);

            foreach (var menu in menus)
            {
                if (menu.NodeLevel != mainNodeLevel)
                {
                    break;
                }

                var menuGroup = GetMenuGroup(menus, menu);
                if (menuGroup != null)
                {
                    menuGroups.Add(menuGroup);
                }
            }

            return menuGroups;
        }

        private List<MenuGroup> GetMenuGroups(IReadOnlyCollection<LinkViewModel> menus, string parentAliasPath,
            LinkViewModel mainMenu = null)
        {
            if (menus.IsNullOrEmpty() || parentAliasPath.IsNullOrEmpty())
            {
                return null;
            }

            var nodeLevel = parentAliasPath.Count(f => f == '/');

            if (mainMenu != null && mainMenu.LinkGroupType == GlobalConstants.LINK_GROUP_GROUPED)
            {
                var groups = menus
                    .Where(a => a.NodeAliasPath.StartsWith(parentAliasPath.EndWithSlash()) &&
                                a.NodeLevel == nodeLevel + 1).OrderBy(a => a.NodeLevel).Select(m => new MenuGroup
                    {
                        Link = _mapper.Map<LinkViewModel>(m)
                    }).ToList();

                if (groups.IsNullOrEmpty())
                {
                    return null;
                }

                for (var i = 0; i < groups.Count; i++)
                {
                    groups[i].ChildLinks = menus.Where(a => a.NodeParentId == groups[i].Link?.NodeId).ToList();
                }

                return groups;
            }

            var childLinks = menus
                .Where(a => a.NodeAliasPath.StartsWith(parentAliasPath.EndWithSlash()) &&
                            a.NodeLevel == nodeLevel + 1).OrderBy(a => a.NodeOrder).ToList();

            if (childLinks.IsNullOrEmpty())
            {
                return null;
            }

            return new List<MenuGroup>
            {
                new MenuGroup {ChildLinks = childLinks}
            };
        }

        private static MenuGroup GetMenuGroup(IReadOnlyCollection<LinkViewModel> menus, LinkViewModel menu)
        {
            if (menus.IsNullOrEmpty() || menu == null)
            {
                return null;
            }

            var nodeLevel = menu.NodeAliasPath.Count(f => f == '/');

            var childLinks = menus
                .Where(a => a.NodeAliasPath.StartsWith(menu.NodeAliasPath.EndWithSlash()) &&
                            a.NodeLevel == nodeLevel + 1).OrderBy(a => a.NodeOrder).ToList();

            if (childLinks.IsNullOrEmpty())
            {
                return new MenuGroup {Link = menu};
            }

            return new MenuGroup
            {
                Link = menu,
                ChildLinks = childLinks
            };
        }

        public List<LinkViewModel> GetFooterBottomMenus()
        {
            return _mapper.Map<List<LinkViewModel>>(_cacheService.GetDocumentDependentOnChildrenPath(
                () => _linkRepository.GetLinks(PathConstants.PATH_FOOTER_BOTTOM_MENU), "GetFooterTopMenus",
                PathConstants.PATH_FOOTER_BOTTOM_MENU));
        }
    }
}
using System.Collections.Generic;
using ETG.Web.Models.Menu;

namespace ETG.Web.Services.Menu
{
    public interface IMenuService
    {
        List<MainMenuViewModel> GetMainMenuViewModel();
        List<MenuGroup> GetMenus(string path);
        List<MenuGroup> GetFooterTopMenus();
        List<LinkViewModel> GetFooterBottomMenus();
    }
}

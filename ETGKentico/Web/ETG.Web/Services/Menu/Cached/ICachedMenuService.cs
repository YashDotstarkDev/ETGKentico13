using System.Collections.Generic;
using ETG.Web.Models.Common;
using ETG.Web.Models.Menu;

namespace ETG.Web.Services.Menu.Cached
{
    public interface ICachedMenuService
    {
        List<MainMenuViewModel> GetMainMenuViewModel();

        List<MenuGroup> GetFooterTopMenus();

        List<LinkViewModel> GetFooterBottomMenus();

        ProofPointComponentViewModel GetFooterProofPoints();
    }
}

using System.Web.Mvc;
using ETG.Web.Services.Menu.Cached;


namespace ETG.Web.Controllers.Partial
{
    public class ExperiencesController : Controller
    {
        private readonly ICachedMenuService _menuService;
        public ExperiencesController(ICachedMenuService menuService)
        {
            _menuService = menuService;
        }

        

        [ChildActionOnly]
        public ActionResult ExperiencesMatters()
        {
            return View("Partial/Experiences/_ExperiencesMatters", _menuService.GetFooterProofPoints());
        }
    }
}
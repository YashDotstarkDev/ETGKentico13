using ETG.Web.Controllers;
using Kentico.PageBuilder.Web.Mvc;
using System.Web.Mvc;

[assembly: RegisterSection("ETG.Sections.TableSection", typeof(TableSectionController), "Table section")]
namespace ETG.Web.Controllers
{
    public class TableSectionController : Controller
    {
        // GET: FormSertion
        public ActionResult Index()
        {
            return PartialView("Partial/Sections/_TableSection");
        }
    }
}
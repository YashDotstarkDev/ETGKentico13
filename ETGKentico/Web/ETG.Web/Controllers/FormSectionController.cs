using ETG.Web.Controllers;
using Kentico.PageBuilder.Web.Mvc;
using System.Web.Mvc;

[assembly: RegisterSection("ETG.Sections.FormSection", typeof(FormSectionController), "Form section")]
namespace ETG.Web.Controllers
{
    public class FormSectionController : Controller
    {
        // GET: FormSertion
        public ActionResult Index()
        {
            return PartialView("Partial/Sections/_FormSection");
        }
    }
}
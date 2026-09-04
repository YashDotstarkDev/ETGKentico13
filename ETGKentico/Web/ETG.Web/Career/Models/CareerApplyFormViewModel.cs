using ETG.Web.Models;
using System.Web;
using ETG.Web.Models.Base;

namespace ETG.Web.Career.Models
{
    public class CareerApplyFormViewModel : BaseFormViewModel,IViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public string PreferredContactType { get; set; }
        public HttpPostedFileBase CVFile { get; set; }
        public string Message { get; set; }
        public string Role { get; set; }
        public string PageAlias { get; set; }
    }
}
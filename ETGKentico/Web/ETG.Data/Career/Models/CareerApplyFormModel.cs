using Devotion.Automapper.Common;
using System.Web;

namespace ETG.Data.Career.Models
{
    public class CareerApplyFormModel : IDataModel
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
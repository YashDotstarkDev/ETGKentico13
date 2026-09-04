using Castle.Core.Internal;
using ETG.Web.Models;
using System.Collections.Generic;
using System.Linq;
using ETG.Web.Models.Base;

namespace ETG.Web.Brochure.Models
{
    public class BrochureOrderFormViewModel: BaseFormViewModel, IViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public List<BrochureViewModel> BrochureOrders { get; set; }

        public string BrochureGuids { get;set; }
    }
}

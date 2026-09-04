using ETG.Data.Models.Common;
using System.Collections.Generic;

namespace ETG.Web.Models.Common
{
    public class GenericEnquireSidebarViewModel : IViewModel
    {
        public List<EnquiryContactViewModel> EnquiryContacts { get; set; }
    }
}
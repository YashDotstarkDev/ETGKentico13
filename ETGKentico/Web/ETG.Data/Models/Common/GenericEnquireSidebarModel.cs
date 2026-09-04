using Devotion.Automapper.Common;
using ETG.Data.Models.Common;
using System.Collections.Generic;

namespace ETG.Web.Models.Common
{
    public class GenericEnquireSidebarModel : IDataModel
    {
        public List<EnquiryContactModel> EnquiryContacts { get; set; }
    }
}
using ETG.Data.Models.Common;
using ETG.Web.Models.Common;
using System.Collections.Generic;

namespace ETG.Data.Repositories
{
    public interface IContactRepository
    {
        ContactModel GetContact(string path);

        GenericEnquireSidebarModel GenericEnquireSidebar();
    }
}

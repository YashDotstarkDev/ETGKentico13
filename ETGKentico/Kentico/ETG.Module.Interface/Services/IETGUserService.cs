using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Membership;

namespace ETG.Module.Interface.Services
{
    public interface IETGUserService
    {
        List<string> GetCurrentLoginDestinations(UserInfo user);
        List<UserInfo> GetAssignees(string destinationName = "");
    }
}

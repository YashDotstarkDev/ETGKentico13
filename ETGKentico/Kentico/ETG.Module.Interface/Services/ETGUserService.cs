using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CMS.Membership;
using ETG.Core.Kentico;
using ETG.Module.Classes.Info;
using ETG.Module.Classes.Providers;
using ETG.Module.Data;

namespace ETG.Module.Interface.Services
{
    public class ETGUserService : IETGUserService
    {
        private readonly ISiteContext _siteContext;
        private readonly List<RoleDestinationLinkInfo> _allRoleDestinationLinkInfo;
        private readonly List<RoleInfo> _allRoles;
        
        public ETGUserService(ISiteContext siteContext)
        {
            _siteContext = siteContext;
            _allRoleDestinationLinkInfo = RoleDestinationLinkInfoProvider.GetRoleDestinationLinks().ToList();
            _allRoles = RoleInfoProvider.GetRoles().ToList();
        }

        public List<string> GetCurrentLoginDestinations(UserInfo user)
        {
            var roleIds = UserRoleInfoProvider.GetUserRoles().WhereEquals("UserID", user.UserID).Select(a=>a.RoleID).ToList();

            if (roleIds.IsNullOrEmpty())
            {
                return null;
            }

            var roleGuids = RoleInfoProvider.GetRoles().WhereIn("RoleID", roleIds).Select(a => a.RoleGUID).ToList();
            return RoleDestinationLinkInfoProvider.GetRoleDestinationLinks()
                .Where(a => roleGuids.Contains(a.RoleDestinationRoleGuid)).Select(a => a.RoleDestinationDestinations).ToList();

        }

        public List<UserInfo> GetAssignees(string destinationName = "")
        {
            var returnList = new List<UserInfo>();
            if (!destinationName.IsNullOrEmpty())
            {
                var roleGuids = _allRoleDestinationLinkInfo
                    .Where(a => a.RoleDestinationName == destinationName)
                    .Select(a => a.RoleDestinationRoleGuid)
                    .ToList();

                var roleIds = _allRoles
                    .Where(a => roleGuids.Contains(a.RoleGUID) || a.RoleName == ETGUserRole.ENQUIRY_MANAGEMENT_USER)
                    .Select(a => a.RoleID)
                    .ToList();

                returnList = UserInfoProvider.GetUsers()
                    .Columns("CMS_User.*")
                    .Source(sourceItem =>
                        sourceItem.Join<UserRoleInfo>(nameof(UserInfo.UserID), nameof(UserRoleInfo.UserID)))
                    .WhereIn(nameof(UserRoleInfo.RoleID), roleIds)
                    .ToList();

            }

            if (destinationName.IsNullOrEmpty() || returnList.IsNullOrEmpty())
            {
                var role1 = RoleInfoProvider.GetRoleInfo(ETGUserRole.ENQUIRY_MANAGEMENT_USER, _siteContext.SiteName);
                
                if (role1 == null)
                {
                    return null;
                }

                returnList = UserInfoProvider.GetUsers()
                    .Columns("CMS_User.*")
                    .Source(sourceItem =>
                        sourceItem.Join<UserRoleInfo>(nameof(UserInfo.UserID), nameof(UserRoleInfo.UserID)))
                    .WhereEquals(nameof(UserRoleInfo.RoleID), role1?.RoleID ?? 0)
                    .ToList();
            }

            return returnList;
        }
    }
}

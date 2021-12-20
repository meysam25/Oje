using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Hubs.Models
{
    public class UserInfo
    {
        static List<UserInfo> allNotificationUsers = null;

        public string connectionId { get; set; }
        public LoginUserVM loginUserObj { get; set; }


        public static void addNewUser(LoginUserVM newUser, string connectionId)
        {
            if (allNotificationUsers == null)
                allNotificationUsers = new();
            lock (allNotificationUsers)
            {
                allNotificationUsers.Add(new UserInfo() { connectionId = connectionId, loginUserObj = newUser });
            }
        }

        public static void removeUserBy(string connectionId)
        {
            if (allNotificationUsers == null)
                allNotificationUsers = new();
            lock (allNotificationUsers)
            {
                var foundItem = allNotificationUsers.Where(t => t!= null && t.connectionId == connectionId).FirstOrDefault();
                if (foundItem != null)
                    allNotificationUsers.Remove(foundItem);
            }
        }

        internal static List<UserInfo> GetBy(List<long> userIds, int siteSettingId)
        {
            if (allNotificationUsers == null)
                allNotificationUsers = new();
            lock (allNotificationUsers)
            {
                return allNotificationUsers.Where(t => t != null && t.loginUserObj != null && t.loginUserObj.siteSettingId == siteSettingId && userIds.Contains(t.loginUserObj.UserId)).ToList();
            }
        }
    }
}

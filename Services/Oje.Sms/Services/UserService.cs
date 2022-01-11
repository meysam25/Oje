using Oje.Sms.Interfaces;
using Oje.Sms.Models.DB;
using Oje.Sms.Models.View;
using Oje.Sms.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Services
{
    public class UserService : IUserService
    {
        readonly SmsDBContext db = null;
        public UserService(SmsDBContext db)
        {
            this.db = db;
        }

        public User GetBy(int? siteSettingId, string username)
        {
            return db.Users.Where(t => t.SiteSettingId == siteSettingId && t.Username == username).FirstOrDefault();
        }

        public string GetUserFullName(int? siteSettingId, long? userId)
        {
            return db.Users.Where(t => t.SiteSettingId == siteSettingId && t.Id == userId).Select(t => t.Firstname + " " + t.Lastname).FirstOrDefault();
        }

        public List<RoleUsersVM> GetUserFullNameAndMobile(List<long> userIds, int? siteSettingId)
        {
            return db.Users
                .Where(t => t.SiteSettingId == siteSettingId && !string.IsNullOrEmpty(t.Mobile) && userIds.Contains(t.Id))
                .Select(t => new RoleUsersVM
                {
                    mobile = t.Mobile,
                    userFullname = t.Firstname + " " + t.Lastname,
                    userId = t.Id
                })
                .ToList();
        }
    }
}

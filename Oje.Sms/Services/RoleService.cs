using Oje.Sms.Interfaces;
using Oje.Sms.Models.View;
using Oje.Sms.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Services
{
    public class RoleService : IRoleService
    {
        readonly SmsDBContext db = null;
        public RoleService(SmsDBContext db)
        {
            this.db = db;
        }

        public List<RoleUsersVM> GetUsersBy(List<int> roleIds, int? siteSettingId, int count)
        {
            return db.UserRoles
                .Where(t => t.User.Mobile != null && roleIds.Contains(t.RoleId) && t.User.SiteSettingId == siteSettingId)
                .Select(t => new RoleUsersVM
                {
                    userId = t.UserId,
                    userFullname = t.User.Firstname + " " + t.User.Lastname,
                    mobile = t.User.Mobile
                }).Take(count).ToList();
        }
    }
}

using Oje.EmailService.Interfaces;
using Oje.EmailService.Models.View;
using Oje.EmailService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Services
{
    public class RoleService : IRoleService
    {
        readonly EmailServiceDBContext db = null;
        public RoleService(EmailServiceDBContext db)
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
                    email = t.User.Email
                }).Take(count).ToList();
        }
    }
}

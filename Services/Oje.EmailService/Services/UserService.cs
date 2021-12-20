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
    public class UserService : IUserService
    {
        readonly EmailServiceDBContext db = null;
        public UserService(EmailServiceDBContext db)
        {
            this.db = db;
        }

        public string GetUserFullName(int? siteSettingId, long? userId)
        {
            return db.Users.Where(t => t.SiteSettingId == siteSettingId && t.Id == userId).Select(t => t.Firstname + " " + t.Lastname).FirstOrDefault();
        }

        public List<RoleUsersVM> GetUserFullNameAndEmail(List<long> userIds, int? siteSettingId)
        {
            return db.Users
                .Where(t => t.SiteSettingId == siteSettingId && !string.IsNullOrEmpty(t.Mobile) && userIds.Contains(t.Id))
                .Select(t => new RoleUsersVM
                {
                    email = t.Email,
                    userFullname = t.Firstname + " " + t.Lastname,
                    userId = t.Id
                })
                .ToList();
        }
    }
}

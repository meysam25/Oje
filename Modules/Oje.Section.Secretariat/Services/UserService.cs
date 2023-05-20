using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Secretariat.Interfaces;
using Oje.Section.Secretariat.Models.DB;
using Oje.Section.Secretariat.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Secretariat.Services
{
    public class UserService : IUserService
    {
        readonly SecretariatDBContext db = null;
        readonly IRoleService RoleService = null;
        public UserService
            (
                SecretariatDBContext db,
                IRoleService RoleService
            )
        {
            this.db = db;
            this.RoleService = RoleService;
        }

        public bool Exist(long? id, int? siteSettingId)
        {
            return db.Users.Any(t => t.Id == id && t.SiteSettingId == siteSettingId);
        }

        public long? GetOrCreateByUsername(string mobile, int siteSettingId, string fullname, long userId)
        {
            long? result = null;

            if (!string.IsNullOrEmpty(mobile) && mobile.IsMobile() && siteSettingId.ToIntReturnZiro() > 0 && !string.IsNullOrEmpty(fullname) && userId > 0)
            {
                result = db.Users
                    .Where(t => t.Mobile == mobile && t.SiteSettingId == siteSettingId)
                    .Select(t => t.Id)
                    .FirstOrDefault();
                if (result.ToIntReturnZiro() <= 0)
                {
                    int roleId = RoleService.GetCreate("Secretariat", "دبیرخانه", 1);
                    if (roleId > 0)
                    {
                        string firstname = fullname.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries)[0];
                        string lastname = fullname.Replace(firstname, "");
                        if (string.IsNullOrEmpty(firstname))
                            firstname = " ";
                        if (string.IsNullOrEmpty(lastname)) 
                            lastname = " ";

                        var newUser = new User() 
                        {
                            Username = mobile,
                            Firstname = firstname,
                            Lastname = lastname,
                            Password = RandomService.GeneratePassword(10).GetSha1(),
                            IsActive = true,
                            CreateDate = DateTime.Now,
                            ParentId = userId,
                            Mobile = mobile,
                            SiteSettingId = siteSettingId,
                        };
                        db.Entry(newUser).State = EntityState.Added;
                        db.SaveChanges();

                        var newUserRole = new UserRole() { Id = Guid.NewGuid(), RoleId = roleId, UserId = newUser.Id };
                        newUserRole.FilledSignature();
                        newUser.FilledSignature();
                        
                        db.Entry(newUserRole).State = EntityState.Added;
                        db.SaveChanges();

                        result = newUser.Id;
                    }
                    
                }
            }

            return result;
        }

        public object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.Users.Where(t => t.SiteSettingId == siteSettingId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => (t.Firstname + " " + t.Lastname + "(" + t.Username + ")").Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Firstname + " " + t.Lastname + "(" + t.Username + ")" }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}

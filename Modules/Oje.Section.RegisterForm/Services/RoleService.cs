using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.RegisterForm.Services
{
    public class RoleService : IRoleService
    {
        readonly RegisterFormDBContext db = null;
        public RoleService(RegisterFormDBContext db)
        {
            this.db = db;
        }

        public object GetLightList(int? siteSettingId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.Roles.Where(t => t.SiteSettingId == null || t.SiteSettingId == siteSettingId).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }

        public object GetList(int? siteSettingId, Select2SearchVM searchInput)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.Roles.Where(t => t.SiteSettingId == null || t.SiteSettingId == siteSettingId);
            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => (t.Title).Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}

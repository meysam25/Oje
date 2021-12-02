using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.Security.Interfaces;
using Oje.Section.Security.Services.EContext;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oje.Section.Security.Services
{
    public class RoleManager : IRoleManager
    {
        readonly SecurityDBContext db = null;
        public RoleManager(SecurityDBContext db)
        {
            this.db = db;
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.Roles.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}

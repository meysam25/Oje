using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Services.EContext;
using System.ComponentModel.DataAnnotations;

namespace Oje.Security.Services
{
    public class RoleService : IRoleService
    {
        readonly SecurityDBContext db = null;
        public RoleService(SecurityDBContext db)
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

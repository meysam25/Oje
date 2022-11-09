using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.GlobalForms.Services
{
    public class RoleService : IRoleService
    {
        readonly GeneralFormDBContext db = null;
        public RoleService
            (
                GeneralFormDBContext db
            )
        {
            this.db = db;
        }

        public object GetLightList()
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.Roles.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}

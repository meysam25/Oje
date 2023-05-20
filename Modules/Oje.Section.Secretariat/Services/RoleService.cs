using Oje.Section.Secretariat.Interfaces;
using Oje.Section.Secretariat.Models.DB;
using Oje.Section.Secretariat.Services.EContext;
using System.Linq;

namespace Oje.Section.Secretariat.Services
{
    public class RoleService: IRoleService
    {
        readonly SecretariatDBContext db = null;
        public RoleService
            (
                SecretariatDBContext db
            )
        {
            this.db = db;
        }

        public int GetCreate(string name, string title, long value)
        {
            int result = 0;

            result = db.Roles.Where(t => t.Name == name && t.SiteSettingId == null).Select(t => t.Id).FirstOrDefault();
            if (result <= 0)
            {
                var newRole = new Role() 
                {
                    Name = name,
                    Title = title,
                    Value = value,
                    DisabledOnlyMyStuff = true,
                };
                db.Entry(newRole).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.SaveChanges();
                result = newRole.Id;

                newRole.FilledSignature();
                db.SaveChanges();
            }

            return result;
        }
    }
}

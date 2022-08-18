using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Sanab.Interfaces;
using Oje.Sms.Services.EContext;

namespace Oje.Sanab.Services
{
    public class CompanyService : ICompanyService
    {
        readonly SanabDBContext db = null;
        public CompanyService
            (
                SanabDBContext db
            )
        {
            this.db = db;
        }

        public bool Exist(int id)
        {
            return db.Companies.Any(t => t.Id == id);
        }

        public object GetLightList()
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.Companies.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}

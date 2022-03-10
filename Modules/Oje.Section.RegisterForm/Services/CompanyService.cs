using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.DB;
using Oje.Section.RegisterForm.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.RegisterForm.Services
{
    public class CompanyService : ICompanyService
    {
        readonly RegisterFormDBContext db = null;
        public CompanyService(
                RegisterFormDBContext db
            )
        {
            this.db = db;
        }

        public Company GetByUserFilledRegisterFormId(long id)
        {
            return db.UserFilledRegisterFormCompanies.Where(t => t.UserFilledRegisterFormId == id).Select(t => t.Company).FirstOrDefault();
        }

        public Company GetByUserId(long userId)
        {
            return db.UserCompanies.Where(t => t.UserId == userId).Select(t => t.Company).FirstOrDefault();
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.Companies.Where(t => t.IsActive == true).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }

        public string GetTitleById(int id)
        {
            return db.Companies.Where(t => t.IsActive == true && t.Id == id).Select(t => t.Title).FirstOrDefault();
        }
    }
}

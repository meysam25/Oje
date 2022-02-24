using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oje.AccountService.Services
{
    public class CompanyService: ICompanyService
    {
        readonly AccountDBContext db = null;
        public CompanyService(AccountDBContext db)
        {
            this.db = db;
        }

        public Company GetBy(string companyTitle)
        {
            return db.Companies.Where(t => t.Title == companyTitle).FirstOrDefault();
        }

        public int GetIdBy(string companyTitle)
        {
            return db.Companies.Where(t => t.Title == companyTitle).Select(t => t.Id).FirstOrDefault();
        }

        public object GetightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.Companies.Select(t => new 
            {
                id = t.Id,
                title = t.Title
            }).ToList());

            return result;
        }
    }
}

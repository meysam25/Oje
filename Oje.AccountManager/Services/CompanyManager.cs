using Oje.AccountManager.Interfaces;
using Oje.AccountManager.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oje.AccountManager.Services
{
    public class CompanyManager: ICompanyManager
    {
        readonly AccountDBContext db = null;
        public CompanyManager(AccountDBContext db)
        {
            this.db = db;
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

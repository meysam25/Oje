using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.FireBaseData.Interfaces;
using Oje.Section.FireBaseData.Services.EContext;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Oje.Section.FireBaseData.Services
{
    public class CompanyService : ICompanyService
    {
        readonly FireBaseDataDBContext db = null;
        public CompanyService(FireBaseDataDBContext db)
        {
            this.db = db;
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetAttribute<DisplayAttribute>()?.Name } };

            result.AddRange(db.Companies.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}

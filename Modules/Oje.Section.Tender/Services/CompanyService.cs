using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.Tender.Services
{
    public class CompanyService : ICompanyService
    {
        readonly TenderDBContext db = null;
        public CompanyService(TenderDBContext db)
        {
            this.db = db;
        }

        public object GetLightList(long? userId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange
                (
                    db.Companies
                    .Where(t => t.UserCompanies.Any(t => t.UserId == userId))
                    .Select(t => new 
                    { 
                        id = t.Id, 
                        title = t.Title 
                    }).ToList()
                );

            return result;
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange
                (
                    db.Companies
                    .Select(t => new
                    {
                        id = t.Id,
                        title = t.Title
                    }).ToList()
                );

            return result;
        }
    }
}

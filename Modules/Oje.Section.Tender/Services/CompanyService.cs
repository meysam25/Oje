using Oje.Infrastructure;
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
                    .Where(t => t.IsActive == true && t.UserCompanies.Any(t => t.UserId == userId))
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
            List<object> result = new ();

            result.AddRange
                (
                    db.Companies
                    .Where(t => t.IsActive == true && t.Id > 0)
                    .Select(t => new
                    {
                        id = t.Id,
                        title = t.Title,
                        src = GlobalConfig.FileAccessHandlerUrl + t.Pic64
                    }).ToList()
                );

            return result;
        }

        public object GetLightListString()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange
               (
                   db.Companies
                   .Where(t => t.IsActive == true && t.Id > 0)
                   .Select(t => new
                   {
                       id = t.Title,
                       title = t.Title
                   }).ToList()
               );

            return result;
        }
    }
}

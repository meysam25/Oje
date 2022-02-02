using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Services.EContext;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Services
{
    public class CompanyService : ICompanyService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public CompanyService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public object GetLightList()
        {
            List<object> result = new();

            result.AddRange(db.Companies.Select(t => new { id = t.Id, title = t.Title, src = GlobalConfig.FileAccessHandlerUrl + t.Pic32 }).ToList());

            return result;
        }
    }
}

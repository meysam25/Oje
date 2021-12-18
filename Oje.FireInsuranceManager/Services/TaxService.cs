using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.DB;
using Oje.FireInsuranceService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Services
{
    public class TaxService : ITaxService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public TaxService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public Tax GetLastActiveItem()
        {
            return db.Taxs.OrderBy(t => t.Year).Where(t => t.IsActive == true).AsNoTracking().FirstOrDefault();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceManager.Interfaces;
using Oje.FireInsuranceManager.Models.DB;
using Oje.FireInsuranceManager.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Services
{
    public class TaxManager : ITaxManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public TaxManager(FireInsuranceManagerDBContext db)
        {
            this.db = db;
        }

        public Tax GetLastActiveItem()
        {
            return db.Taxs.OrderBy(t => t.Year).Where(t => t.IsActive == true).AsNoTracking().FirstOrDefault();
        }
    }
}

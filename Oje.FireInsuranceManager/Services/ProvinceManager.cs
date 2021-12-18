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
    public class ProvinceService : IProvinceService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public ProvinceService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public Province GetProvinceById(int? id)
        {
            return db.Provinces.Where(t => t.Id == id && t.IsActive == true).AsNoTracking().FirstOrDefault();
        }
    }
}

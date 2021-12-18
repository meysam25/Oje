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
    public class CityService: ICityService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public CityService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public City GetById(int? cityId)
        {
            return db.Cities.Where(t => t.Id == cityId && t.IsActive == true).AsNoTracking().FirstOrDefault();
        }
    }
}

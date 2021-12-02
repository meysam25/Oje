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
    public class CityManager: ICityManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public CityManager(FireInsuranceManagerDBContext db)
        {
            this.db = db;
        }

        public City GetById(int? cityId)
        {
            return db.Cities.Where(t => t.Id == cityId && t.IsActive == true).AsNoTracking().FirstOrDefault();
        }
    }
}

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
    public class ProvinceManager : IProvinceManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public ProvinceManager(FireInsuranceManagerDBContext db)
        {
            this.db = db;
        }

        public Province GetProvinceById(int? id)
        {
            return db.Provinces.Where(t => t.Id == id && t.IsActive == true).AsNoTracking().FirstOrDefault();
        }
    }
}

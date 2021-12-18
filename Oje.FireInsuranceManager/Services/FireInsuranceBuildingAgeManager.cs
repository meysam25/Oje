using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.DB;
using Oje.FireInsuranceService.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Services
{
    public class FireInsuranceBuildingAgeService : IFireInsuranceBuildingAgeService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public FireInsuranceBuildingAgeService(FireInsuranceServiceDBContext db)
        {
            this.db = db;
        }

        public FireInsuranceBuildingAge GetById(int? id)
        {
            return db.FireInsuranceBuildingAges.Where(t => t.IsActive == true && t.Id == id).AsNoTracking().FirstOrDefault();
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.FireInsuranceBuildingAges.Where(t => t.IsActive == true).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}

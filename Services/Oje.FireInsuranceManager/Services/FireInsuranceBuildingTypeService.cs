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
    public class FireInsuranceBuildingTypeService : IFireInsuranceBuildingTypeService
    {
        readonly FireInsuranceServiceDBContext db = null;
        public FireInsuranceBuildingTypeService(
                FireInsuranceServiceDBContext db
            )
        {
            this.db = db;
        }

        public FireInsuranceBuildingType GetById(int? id)
        {
            return db.FireInsuranceBuildingTypes.Where(t => t.IsActive == true && t.Id == id).AsNoTracking().FirstOrDefault();
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.FireInsuranceBuildingTypes.Where(t => t.IsActive == true).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}

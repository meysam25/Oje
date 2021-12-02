using Microsoft.EntityFrameworkCore;
using Oje.FireInsuranceManager.Interfaces;
using Oje.FireInsuranceManager.Models.DB;
using Oje.FireInsuranceManager.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Services
{
    public class FireInsuranceBuildingBodyManager : IFireInsuranceBuildingBodyManager
    {
        readonly FireInsuranceManagerDBContext db = null;
        public FireInsuranceBuildingBodyManager(
            FireInsuranceManagerDBContext db
            )
        {
            this.db = db;
        }

        public FireInsuranceBuildingBody GetById(int? id)
        {
            return db.FireInsuranceBuildingBodies.Where(t => t.IsActive == true && t.Id == id).AsNoTracking().FirstOrDefault();
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.FireInsuranceBuildingBodies
                .Where(t => t.IsActive == true)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title
                }).ToList());

            return result;
        }
    }
}

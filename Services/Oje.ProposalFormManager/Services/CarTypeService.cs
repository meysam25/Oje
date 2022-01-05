using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Services
{
    public class CarTypeService : ICarTypeService
    {
        readonly ProposalFormDBContext db = null;
        public CarTypeService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public CarType GetById(int? carTypeId, int? vehicleTypeId)
        {
            return db.VehicleTypeCarTypes
                .Where(t => t.VehicleTypeId == vehicleTypeId && t.CarTypeId == carTypeId && t.CarType.IsActive == true && t.VehicleType.IsActive == true)
                .Select(t => t.CarType)
                .FirstOrDefault();
        }

        public object GetLightList(int vehicleTypeId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.VehicleTypeCarTypes.Where(t => t.VehicleType.IsActive == true && t.CarType.IsActive == true && t.VehicleTypeId == vehicleTypeId).Select(t => t.CarType).OrderBy(t => t.Order).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}

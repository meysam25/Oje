using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.DB;
using Oje.ProposalFormService.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormService.Services
{
    public class VehicleUsageService : IVehicleUsageService
    {
        readonly ProposalFormDBContext db = null;
        public VehicleUsageService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public VehicleUsage GetById(int? id)
        {
            return db.VehicleUsages.Where(t => t.Id == id).AsNoTracking().FirstOrDefault();
        }

        public object GetLightList(int? vehicleSystemId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            if (vehicleSystemId.ToIntReturnZiro() > 0)
                result.AddRange(
                    db.VehicleUsages
                    .Where(t => t.IsActive == true && t.VehicleUsageCarTypes.
                                                        Any(tt => tt.CarType.IsActive == true && tt.CarType.VehicleSystems.
                                                                Any(ttt => ttt.IsActive == true && ttt.Id == vehicleSystemId)))
                    .Select(t => new { id = t.Id, title = t.Title }).ToList()
                    );

            return result;
        }
    }
}

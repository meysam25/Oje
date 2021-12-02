using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.DB;
using Oje.ProposalFormManager.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.ProposalFormManager.Services
{
    public class VehicleUsageManager : IVehicleUsageManager
    {
        readonly ProposalFormDBContext db = null;
        public VehicleUsageManager(ProposalFormDBContext db)
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

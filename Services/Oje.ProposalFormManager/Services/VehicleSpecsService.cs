using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure.Models;
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
    public class VehicleSpecsService: IVehicleSpecsService
    {
        readonly ProposalFormDBContext db = null;
        public VehicleSpecsService(ProposalFormDBContext db)
        {
            this.db = db;
        }

        public VehicleSpec GetBy(int? id, int? brandId, int? vehicleTypeId)
        {
            return db.VehicleSpecs
                .OrderBy(t => t.Order)
                .Where(t => t.Id == id && t.IsActive == true && t.VehicleSystem.IsActive == true && t.VehicleSystem.Id == brandId && t.VehicleSystem.VehicleSystemVehicleTypes.Any(tt => tt.VehicleType.IsActive == true && tt.VehicleTypeId == vehicleTypeId))
                .AsNoTracking()
                .FirstOrDefault();
        }

        public object GetSelect2List(Select2SearchVM searchInput, int? vehicleTypeId, int? brandId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.VehicleSpecs
                .OrderBy(t => t.Order)
                .Where(t => t.IsActive == true && t.VehicleSystem.IsActive == true && t.VehicleSystem.Id == brandId && t.VehicleSystem.VehicleSystemVehicleTypes.Any(tt => tt.VehicleType.IsActive == true && tt.VehicleTypeId == vehicleTypeId));

            if (!string.IsNullOrEmpty(searchInput.search))
                qureResult = qureResult.Where(t => t.Title.Contains(searchInput.search));
            qureResult = qureResult.Skip((searchInput.page.Value - 1) * take).Take(take);
            if (qureResult.Count() >= 50)
                hasPagination = true;

            result.AddRange(qureResult.Select(t => new { id = t.Id, text = t.Title }).ToList());

            return new { results = result, pagination = new { more = hasPagination } };
        }
    }
}

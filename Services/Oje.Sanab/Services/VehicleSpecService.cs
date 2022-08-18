using Oje.Infrastructure.Models;
using Oje.Sanab.Interfaces;
using Oje.Sms.Services.EContext;

namespace Oje.Sanab.Services
{
    public class VehicleSpecService : IVehicleSpecService
    {
        readonly SanabDBContext db = null;
        public VehicleSpecService
            (
                SanabDBContext db
            )
        {
            this.db = db;
        }

        public bool Exist(int VehicleSystemId, int id) => db.VehicleSpecs.Any(t => t.VehicleSystemId == VehicleSystemId && t.Id == id);

        public object GetSelect2List(Select2SearchVM searchInput, int? vehicleSystemId)
        {
            List<object> result = new List<object>();

            var hasPagination = false;
            int take = 50;

            if (searchInput == null)
                searchInput = new Select2SearchVM();
            if (searchInput.page == null || searchInput.page <= 0)
                searchInput.page = 1;

            var qureResult = db.VehicleSpecs.OrderBy(t => t.Order).Where(t => t.VehicleSystemId == vehicleSystemId);
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

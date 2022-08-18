using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Sanab.Interfaces;
using Oje.Sms.Services.EContext;

namespace Oje.Sanab.Services
{
    public class VehicleTypeService: IVehicleTypeService
    {
        readonly SanabDBContext db = null;
        public VehicleTypeService
            (
                SanabDBContext db
            )
        {
            this.db = db;
        }

        public bool Exist(int id)
        {
            return db.VehicleTypes.Any(t => t.Id == id);
        }

        public object GetLightList()
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.VehicleTypes.Select(t => new 
            {
                id = t.Id,
                title = t.Title,
            }).ToList());

            return result;
        }
    }
}

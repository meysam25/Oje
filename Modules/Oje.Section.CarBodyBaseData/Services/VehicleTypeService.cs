using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.CarBodyBaseData.Interfaces;
using Oje.Section.CarBodyBaseData.Services.EContext;
using System.Collections.Generic;
using System.Linq;

namespace Oje.Section.CarBodyBaseData.Services
{
    public class VehicleTypeService : IVehicleTypeService
    {
        readonly CarBodyDBContext db = null;
        public VehicleTypeService
            (
                CarBodyDBContext db
            )
        {
            this.db = db;
        }

        public object GetLightList()
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.VehicleTypes.Select(t => new
            {
                id = t.Id,
                title = t.Title
            }).ToList());

            return result;
        }
    }
}

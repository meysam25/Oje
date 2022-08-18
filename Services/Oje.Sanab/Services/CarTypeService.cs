using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Sanab.Interfaces;
using Oje.Sms.Services.EContext;

namespace Oje.Sanab.Services
{
    public class CarTypeService : ICarTypeService
    {
        readonly SanabDBContext db = null;
        public CarTypeService
            (
                SanabDBContext db
            )
        {
            this.db = db;
        }

        public bool Exist(int? ctId)
        {
            return db.CarTypes.Any(t => t.Id == ctId);
        }

        public object GetLightList()
        {
            List<object> result = new() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.CarTypes.Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}

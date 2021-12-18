using Oje.AccountService.Interfaces;
using Oje.AccountService.Services.EContext;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Services
{
    public class CityService: ICityService
    {
        readonly AccountDBContext db = null;
        public CityService(AccountDBContext db)
        {
            this.db = db;
        }

        public object GetLightList(int? provinceId)
        {
            List<object> result = new List<object>() { new { id = "", title = BMessages.Please_Select_One_Item.GetEnumDisplayName() } };

            result.AddRange(db.Cities.Where(t => t.ProvinceId == provinceId).Select(t => new { id = t.Id, title = t.Title }).ToList());

            return result;
        }
    }
}
